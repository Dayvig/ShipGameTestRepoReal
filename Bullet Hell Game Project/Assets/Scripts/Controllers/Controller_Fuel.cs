using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers
{
    public class Controller_Fuel : MonoBehaviour
    {
        public Slider slider;
        public Model_Player playerModel;
        
        public float FuelMax;
        public float currentFuel;
        public float timeInSeconds;
        public float interval;
        public float startTime;
        public float elapsed = 0f;
        public bool spawnGas;
        
        
        public List<GameObject> meterList = new List<GameObject>();
        void Start()
        {
            Debug.Assert(playerModel != null, "Controller_PlayerGuns is looking for a reference to Model_Player, but none has been added in the Inspector!");

            interval = FuelMax / timeInSeconds;
            currentFuel = FuelMax;
            SetFuel(currentFuel);
            spawnGas = false;
            
            startTime = Time.time;
        }
        
        void Update()
        {
            elapsed += Time.deltaTime;
            if (!(elapsed >= 0.1f)) return;
            elapsed %= 0.1f;
            LowerFuel();
            
            
            if (currentFuel <= 0)
            {
                if (playerModel.hitpointsCurrent > 0)
                {
                    playerModel.livesCurrent--;
                    playerModel.hitpointsCurrent = 0;
                }
            }

            for (int i = meterList.Count - 1; i > 0; i--)
            {
                if ((int) Mathf.Round(slider.value / 10) > i)
                {
                    meterList[i].SetActive(true);
                }
                else
                {
                    meterList[i].SetActive(false);
                }
            }
        }

        public void LowerFuel()
        {
            currentFuel -= interval;
            SetFuel(currentFuel);

            if (currentFuel <= 0)
            {
                // Change to losing screen
                if (!playerModel.lostLife)
                {
                    playerModel.livesCurrent--;
                    playerModel.lostLife = true;
                }

                playerModel.hitpointsCurrent = 0;            
            }
            
            UpdateColor();
        }

        public void SetFuel(float amount)
        {
            currentFuel = amount;
            slider.value = amount;
        }

        public void UpdateColor()
        {
            float t = (Mathf.Sin(3*(Time.time - startTime)));
            if (currentFuel > 30)
            {
                foreach (GameObject gameObject in meterList)
                {
                    gameObject.GetComponent<Image>().color = Color.cyan;
                }
            }
            else
            {
                foreach (GameObject gameObject in meterList)
                {
                    gameObject.GetComponent<Image>().color = Color.Lerp(Color.cyan, Color.red, t);
                }
            }
        }
    }
}
