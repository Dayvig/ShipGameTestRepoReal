using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Effects : MonoBehaviour
{
    public Model_Game gameModel;

    private List<ParticleSystem> _explosionsInactive;
    private List<EffectTimer> _explosionsActive;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(gameModel != null, "Controller_Effects is looking for a reference to Model_Game, but none has been added in the Inspector!");
        _explosionsInactive = new List<ParticleSystem>();
        _explosionsActive = new List<EffectTimer>();   
    }

    void Update()
    {
        for (int i = _explosionsActive.Count - 1; i >= 0; i--)
        {
            EffectTimer ET = _explosionsActive[i];
            ET.timer += Time.deltaTime;
            if (ET.timer >= ET.duration)
            {
                _explosionsActive.Remove(ET);
                ET.ps.gameObject.SetActive(false);
                _explosionsInactive.Add(ET.ps);
            }
        }
    }

    public void MakeExplosion(Vector3 where)
    {
        ParticleSystem explosion;
        if (_explosionsInactive.Count > 0)
        {
            explosion = _explosionsInactive[0];
            _explosionsInactive.Remove(explosion);
            explosion.gameObject.SetActive(true);
        }
        else
        {
            explosion = Instantiate(gameModel.explosionPrefab1).GetComponent<ParticleSystem>();
        }
        
        _explosionsActive.Add(new EffectTimer(explosion, 2));
        explosion.transform.position = where;
        explosion.Emit(35);
    }
}

public class EffectTimer
{
    public ParticleSystem ps;
    public float duration;
    public float timer;

    public EffectTimer (ParticleSystem ps, float duration)
    {
        this.ps = ps;
        this.duration = duration;
        timer = 0;
    }
}