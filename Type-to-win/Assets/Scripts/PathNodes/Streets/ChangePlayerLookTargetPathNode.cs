using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerLookTargetPathNode : PathNode
{
    public GameObject player;
    public GameObject lookTarget;


    public override void OnArrival()
    {
        player.GetComponent<ObjectTracker>().SetTarget(lookTarget);
        player.GetComponent<ObjectTracker>().SetTracking(true);
    }
}

