using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTypingEvent : TypingEvent
{
    public GameObject typingManager;
    public GameObject nextTypingEventObject;
    public Transform teleportSpawnPoint;

    public AudioClip passSFX;
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Pass()
    {
        transform.position = teleportSpawnPoint.position;

        typingManager.GetComponent<TypingManager>().SetTypingEvent(
            nextTypingEventObject.GetComponent<TypingEvent>()
        );

        audioSource.clip = passSFX;
        audioSource.Play();
    }

    // Untimed event
    public override void Fail() {}
}
