using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlleyZombieSpawnPathNode : PathNode
{
    public GameObject player;
    public GameObject zombiePrefab;
    public GameObject typingManager;

    public GameObject[] spawnPoints;
    private int zombieTypingEventIndex = 0;
    private GameObject[] zombies = new GameObject[5];

    private bool eventsStarted = false;


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

            if (i == 0) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "misleading detour",
                        "fatally sidetrack",
                        "mistaken prospect"
                        });

                zombie.GetComponent<Zombie>().Spawn(player);
                player.GetComponent<ObjectTracker>().SetTarget(zombies[i].transform.Find("SkeletonTypingEventSpawnPoint").gameObject);
                player.GetComponent<ObjectTracker>().SetTracking(true);
            } else if(i == 1) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "bad part of town",
                        "destitute region",
                        "savage districts",
                        "ivory gang turfs"
                        });

                zombie.GetComponent<Zombie>().Spawn(player, false);
            } else if(i == 2) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "rush",
                        "dash",
                        "race",
                        "hare",
                        "dart"
                        });

                zombie.GetComponent<Zombie>().Spawn(player, false);
            } else if(i == 3) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "tackle",
                        "charge",
                        "chases",
                        "hunter"
                        });

                zombie.GetComponent<Zombie>().Spawn(player, false);
            } else if(i == 4) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "flying fingers",
                        "adeptly typing",
                        "fast lettering",
                        "precision keys"
                        });

                zombie.GetComponent<Zombie>().Spawn(player, false);
            }
        }
    }


    public override void startNextTypingEvent()
    {
        zombieTypingEventIndex++;

        if (zombieTypingEventIndex >= zombies.Length) {
            player.GetComponent<PathFollower>().SetTraversing(true);
            player.GetComponent<ObjectTracker>().SetTracking(false);

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

