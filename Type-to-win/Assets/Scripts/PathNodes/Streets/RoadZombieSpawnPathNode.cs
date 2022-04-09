using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadZombieSpawnPathNode : PathNode
{
    public GameObject player;
    public GameObject zombiePrefab;
    public GameObject typingManager;
    public GameObject playerSubwayLookInObject;

    public GameObject[] spawnPoints;
    private int zombieTypingEventIndex = 0;
    private GameObject[] zombies = new GameObject[4];

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
                        "discover safety",
                        "seek out solace",
                        "search security"
                        });

                zombie.GetComponent<Zombie>().Spawn(player);
                player.GetComponent<ObjectTracker>().SetTarget(zombies[i].transform.Find("SkeletonTypingEventSpawnPoint").gameObject);
                player.GetComponent<ObjectTracker>().SetTracking(true);
            } else if(i == 1) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "escape streets",
                        "descend subway",
                        "train stations",
                        "transit travel",
                        "enter a tunnel"
                        });

                zombie.GetComponent<Zombie>().Spawn(player, false);
            } else if(i == 2) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "late for work",
                        "tardiness job",
                        "heavy traffic",
                        "rough commute",
                        "difficult day"
                        });

                zombie.GetComponent<Zombie>().Spawn(player, false);
            } else if(i == 3) {
                zte.Initialize(
                        this,
                        player,
                        new string[] {
                        "rumbling earth",
                        "unstable roads",
                        "planet quaking",
                        "seismic scream"
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
            player.GetComponent<ObjectTracker>().SetTarget(playerSubwayLookInObject);

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
