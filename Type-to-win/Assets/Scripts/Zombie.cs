using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieState { Spawning, Chasing, Idle, Dying };

public class Zombie : MonoBehaviour
{
    private GameObject player;

    private ZombieState currentState;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentPositionTimer;
    private float endPositionTime;
    private bool autoChase;

    
    // Update is called once per frame
    void Update()
    {
        if (currentState == ZombieState.Chasing) {
            Transform playerHitbox = player.transform.Find("SkeletonDamagePosition");        

            endPosition = new Vector3(
                playerHitbox.position.x,
                transform.position.y,
                playerHitbox.position.z
            );

            currentPositionTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                currentPositionTimer / endPositionTime
            );
        }

        if (currentState == ZombieState.Spawning) {
            currentPositionTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                currentPositionTimer / endPositionTime
            );

            if (transform.position == endPosition) {
                if (autoChase) {
                    Chase();
                } else {
                    Idle();
                }
            }
        }
    }


    public ZombieState getCurrentState()
    {
        return currentState;
    }


    public void Spawn(GameObject player, bool autoChase = true)
    {
        gameObject.GetComponent<Animator>().SetTrigger("isSpawn");
        //zombieAnimator.SetTrigger("isSpawn");
        // Make skeletons look at player 

        gameObject.GetComponent<ObjectTracker>().rotateYOnly = true;
        gameObject.GetComponent<ObjectTracker>().SetTarget(player);

        this.autoChase = autoChase;
        this.player = player;
        currentState = ZombieState.Spawning;
        currentPositionTimer = 0.0f;
        endPositionTime = 1.5f; // this is in seconds!?
        startPosition = transform.position;
        endPosition = startPosition;
    }


    public void Idle()
    {
        currentState = ZombieState.Idle;
        gameObject.GetComponent<ObjectTracker>().SetTracking(false);
    }


    public void Chase()
    {
        gameObject.GetComponent<Animator>().SetBool("isMove", true);
        gameObject.GetComponent<ObjectTracker>().SetTracking(false);

        currentState = ZombieState.Chasing;
        currentPositionTimer = 0.0f;
        startPosition = transform.position;

        Transform playerHitbox = player.transform.Find("SkeletonDamagePosition");        

        endPosition = new Vector3(
            playerHitbox.position.x,
            transform.position.y,
            playerHitbox.position.z
        );

        endPositionTime = (float) gameObject.GetComponent<ZombieTypingEvent>().eventTime / 1000.0f;
    }


    public void Die()
    {   
        currentState = ZombieState.Dying;
        gameObject.GetComponent<Animator>().SetTrigger("isDead");
        gameObject.GetComponent<ObjectTracker>().SetTracking(false);
    }
}
