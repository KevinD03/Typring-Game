using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerSpeedPathNode : PathNode
{
    public GameObject player;
    public float newPlayerMovementSpeed;
    public bool resetPlayerMovementSpeed;

    public override void OnArrival()
    {
        if (resetPlayerMovementSpeed) {
            player.GetComponent<PathFollower>().ResetMovementSpeed(); 
        } else {
            player.GetComponent<PathFollower>().movementSpeed = 
                newPlayerMovementSpeed;
        }
    }
}

