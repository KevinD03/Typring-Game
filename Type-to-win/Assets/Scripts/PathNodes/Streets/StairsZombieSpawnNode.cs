using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsZombieSpawnNode : PathNode
{
    public GameObject player;
    public GameObject zombiePrefab;
    public GameObject typingManager;
    public GameObject alleyLookInObject;

    public GameObject[] spawnPoints;
    private int zombieTypingEventIndex = 0;
    private GameObject[] zombies = new GameObject[4];

    private bool eventsStarted = false;
    private string[] zombieWordBank = new string[] {
        "astonished",
        "bewildered",
        "shockingly",
        "confusions",
        "revelation",
        "caught out",
        "helplessly",
        "deadly end",
        "ambuscades",
        "perilously",
        "apocalypse",
        "empty clip",
        "solitarily",
        "ammunition",
        "sharpshoot",
        "perceptive",
        "peeled eye",
        "vigilantly",
        "watchfully",
        "creep into"
    };


    void Update()
    {
        if (!eventsStarted && zombies[0] != null) {
            if (zombies[0].GetComponent<Zombie>().getCurrentState() == ZombieState.Chasing) {
                eventsStarted = true;
                typingManager.GetComponent<TypingManager>().SetTypingEvent(zombies[0].GetComponent<ZombieTypingEvent>());
            }
        }
    }


    public override void OnArrival()
    {
        player.GetComponent<PathFollower>().SetTraversing(false);

        for (int i = 0; i < spawnPoints.Length; i++) {

            GameObject zombie = Instantiate(
                zombiePrefab,
                spawnPoints[i].transform.position,
                spawnPoints[i].transform.rotation
            ) as GameObject;

            zombies[i] = zombie;

            ZombieTypingEvent zte 
                = zombies[i].GetComponent<ZombieTypingEvent>();

            zte.Initialize(
                this,
                player,
                zombieWordBank
            );

            if (i == 0) {
                zombie.GetComponent<Zombie>().Spawn(player);
                player.GetComponent<ObjectTracker>().SetTarget(zombies[i].transform.Find("SkeletonTypingEventSpawnPoint").gameObject);
                player.GetComponent<ObjectTracker>().SetTracking(true);
            } else if (i == 1) {
                zombie.GetComponent<Zombie>().Spawn(player, false);
            } else if (i == 2) {
                zombie.GetComponent<Zombie>().Spawn(player, false);
            } else if (i == 3) {
                zombie.GetComponent<Zombie>().Spawn(player, false);
            }
        }
    }


    public override void startNextTypingEvent()
    {
        zombieTypingEventIndex++;

        if (zombieTypingEventIndex >= zombies.Length) {
            player.GetComponent<PathFollower>().SetTraversing(true);
            player.GetComponent<ObjectTracker>().SetTarget(alleyLookInObject);

            return;
        }

        ZombieTypingEvent zte 
            = zombies[zombieTypingEventIndex].GetComponent<ZombieTypingEvent>();

        player.GetComponent<ObjectTracker>().SetTarget(zombies[zombieTypingEventIndex].transform.Find("SkeletonTypingEventSpawnPoint").gameObject);
        player.GetComponent<ObjectTracker>().SetTracking(true);
        zombies[zombieTypingEventIndex].GetComponent<Zombie>().Chase();
        typingManager.GetComponent<TypingManager>().SetTypingEvent(zte);
    }
}

