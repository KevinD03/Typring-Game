using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourEventPathNode : PathNode
{
    public GameObject player;
    public GameObject typingManager;
    public GameObject typingEventObject;


    public override void OnArrival()
    {
        typingManager.GetComponent<TypingManager>().SetTypingEvent(
            typingEventObject.GetComponent<TypingEvent>()
        );

        player.GetComponent<ObjectTracker>().SetTarget(
            typingEventObject
        );

        player.GetComponent<ObjectTracker>().SetTracking(true);
    }
}

