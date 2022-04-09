using System.Collections;
using System.Collections.Generic;
using Collectibles;
using UnityEngine;

public class PortalBehavior : Base_Collectible_Behavior
{
    private Portal values;
    private Model_Player player;
    public float lifeTime;
    public float portalTimer;
    
    public override void MovementUpdate()
    {
        portalTimer += Time.deltaTime;
        if (portalTimer > lifeTime)
        {
            Debug.Log("Portal Expired");
            Destroy(this.gameObject);
        }
    }

    public override void SetupCollectible()
    {
        values = GameObject.Find("Model").GetComponent<Portal>();
        player = GameObject.Find("Model").GetComponent<Model_Player>();
        lifeTime = values.lifeTime;
    }

    public override bool CollectionCondition()
    {
        return CollectWhenMovedInto(values.collectionRange);
    }

    public override void UpdateVisuals()
    {
        
    }

    public override void Collect()
    {
        //enter portal code
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        Debug.Log("Portal Entered");
    }
}
