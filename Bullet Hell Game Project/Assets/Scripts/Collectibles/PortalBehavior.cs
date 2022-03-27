using System.Collections;
using System.Collections.Generic;
using Collectibles;
using UnityEngine;

public class PortalBehavior : Base_Collectible_Behavior
{
    private Portal values;
    private Model_Player player;
    
    public override void MovementUpdate()
    {
        
    }

    public override void SetupCollectible()
    {
        values = GameObject.Find("Model").GetComponent<Portal>();
        player = GameObject.Find("Model").GetComponent<Model_Player>();
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
        Debug.Log("Portal Entered");
    }
}
