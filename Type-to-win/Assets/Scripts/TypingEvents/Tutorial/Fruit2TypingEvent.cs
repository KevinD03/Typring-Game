using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit2TypingEvent : TypingEvent
{
    public Transform teleportSpawnPoint;
    public GameObject player;
    public GameObject path;

    public CharacterController characterController;

    private bool turningToDoor = false;
    private float timeCount = 0.0f;
    private float rotationScale = 5.0f;
    private float maxRotationTimeSeconds = 15.0f;
    public float speed = 1.0f;
    public float smooth = 5.0f;

    public AudioClip passSFX;
    public AudioSource audioSource;


    void Update()
    {
        if (turningToDoor) {
          
            timeCount = timeCount + Time.deltaTime;

            Quaternion facingDoorTarget = Quaternion.Euler(0, 120, 0);
            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation,
                facingDoorTarget,
                //Time.deltaTime * speed
                timeCount / maxRotationTimeSeconds
            //(timeCount * rotationScale) / maxRotationTimeSeconds
            );
        }

        if (timeCount * rotationScale >= maxRotationTimeSeconds && turningToDoor) {
            turningToDoor = false;
            player.GetComponent<PathFollower>().SetPath(path);
        }
    }

    public override void Pass()
    {
        transform.position = teleportSpawnPoint.position;
        turningToDoor = true;

        audioSource.clip = passSFX;
        audioSource.Play();
    }

    public override void Fail()
    {
        turningToDoor = true;
    }
}

