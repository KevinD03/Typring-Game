using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTypingEvent : ProceduralTypingEvent
{
    private PathNode master;
    private GameObject player;


    public void Initialize(PathNode master, GameObject player, string[] promptOptions) {
        this.master = master;
        this.player = player;
        this.promptOptions = promptOptions;

        InitializePTE();
    }


    public override void Pass()
    {
        master.startNextTypingEvent();
        gameObject.GetComponent<Zombie>().Idle();
        transform.position = new Vector3(0, -10, 0);
    }


    public override void Fail()
    {
        player.GetComponent<PathFollower>().takeDamage(34);
        master.startNextTypingEvent();
        gameObject.GetComponent<Zombie>().Idle();
        transform.position = new Vector3(0, -10, 0);
    }
}

