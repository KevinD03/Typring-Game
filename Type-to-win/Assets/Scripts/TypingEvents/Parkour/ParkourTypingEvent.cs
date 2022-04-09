using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourTypingEvent : ProceduralTypingEvent
{
    public GameObject player;

    void Start()
    {
        InitializePTE();
    }

    public override void Pass()
    {
        player.GetComponent<ObjectTracker>().SetTracking(false);
        player.GetComponent<PathFollower>().ResetMovementSpeed(); 
    }

    public override void Fail()
    {
        player.GetComponent<ObjectTracker>().SetTracking(false);
        // TODO: move health management out of follower into own script
        player.GetComponent<PathFollower>().takeDamage(33);
        player.GetComponent<PathFollower>().ResetMovementSpeed(); 
    }
}

