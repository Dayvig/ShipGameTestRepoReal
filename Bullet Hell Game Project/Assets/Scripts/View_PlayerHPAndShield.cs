using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_PlayerHPAndShield : MonoBehaviour
{
    public Model_Player playerModel;

    public Color shieldColor;
    public Color armorColor;
    public Color hitColor; 

    public Image shieldIcon;
    public Image armorIcon;

    public Image shieldBack;
    public Image shieldPoint;
    public Image armorPoint;

    public Vector2 shieldMaxSize;

    private List<Image> _shieldPoints;
    private List<Image> _armorPoints;

    private int _lastArmor;
    private float _lastShield;

    private MeshRenderer[] renderers;
    private ParticleSystem[] jets;

    public float livesOffset;
    public float livesSpace;

    public TextMeshProUGUI scoreDisplay;


    void Start()
    {
        Debug.Assert(playerModel != null, "View_PlayerHPAndShield is looking for a reference to Model_Player, but none has been added in the Inspector!");
        shieldMaxSize = new Vector2(65, 0);

        _shieldPoints = new List<Image>();
        _armorPoints = new List<Image>();
        armorIcon.enabled = false;

        for (int i = 0; i < 30; i++)
        {
            _shieldPoints.Add(Instantiate(shieldPoint, shieldIcon.transform));
            _shieldPoints[i].rectTransform.localPosition = shieldPoint.rectTransform.localPosition + Vector3.up * 30 * i;
            _shieldPoints[i].gameObject.SetActive(false);

            _armorPoints.Add(Instantiate(armorPoint, armorIcon.transform));
            _armorPoints[i].rectTransform.localPosition = (armorPoint.rectTransform.localPosition + Vector3.right * livesSpace * i);
            _armorPoints[i].gameObject.SetActive(false);
        }

        shieldPoint.gameObject.SetActive(false);
        armorPoint.gameObject.SetActive(false);

        renderers = playerModel.ship.GetComponentsInChildren<MeshRenderer>();
        jets = playerModel.ship.GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        _IconFlash();

        float shieldMaxImageHeight = playerModel.shieldPointsMax * 30 + 10;
        shieldMaxSize = Vector2.Lerp(shieldMaxSize, new Vector2(65, shieldMaxImageHeight), .05f);
        shieldBack.rectTransform.sizeDelta = shieldMaxSize;

        int maxPointsToDisplay = Mathf.FloorToInt((shieldMaxSize.y - 10) / 30 + .05f);
        int pointsToDisplay = (int) Mathf.Min(playerModel.shielddPointsCurrent, maxPointsToDisplay);

        for (int i = 0; i < _shieldPoints.Count; i++)
        {
            _shieldPoints[i].gameObject.SetActive(i < pointsToDisplay);
        }

        for (int i = 0; i < _armorPoints.Count; i++)
        {
            _armorPoints[i].gameObject.SetActive(i < playerModel.livesCurrent);
        }

        _ShowAliveAndDead(playerModel.hitpointsCurrent > 0);
        scoreDisplay.text = "" + playerModel.score;  //obj ref not set to instance bug
    }

    private void _IconFlash()
    {
        if (playerModel.shielddPointsCurrent > _lastShield)
            shieldIcon.color = Color.white;
        else if (playerModel.shielddPointsCurrent < _lastShield)
            shieldIcon.color = hitColor;
        else
            shieldIcon.color = Color.Lerp(shieldIcon.color, shieldColor, .04f);
        _lastShield = playerModel.shielddPointsCurrent;
        
        if (playerModel.livesCurrent > _lastArmor)
            armorIcon.color = Color.white;
        else if (playerModel.livesCurrent < _lastArmor)
            armorIcon.color = hitColor;
        else
            armorIcon.color = Color.Lerp(armorIcon.color, armorColor, .04f);
        _lastArmor = playerModel.livesCurrent;
    }

    private void _ShowAliveAndDead(bool isAlive)
    {
        // Offramp to minimize excessive crawling through the array
        if (renderers == null) return;
        if (renderers[0].enabled == isAlive) return;

        foreach (var r in renderers)
            r.enabled = isAlive;

        foreach (var j in jets)
        {
            if (isAlive)
                j.Play();
            else
                j.Stop();
        }
    }
}
