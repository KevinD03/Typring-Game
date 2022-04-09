using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypableATE : RandomTypingEvent
{
    public GameObject typingManager;
    public GameObject nextTypingEventObject;

    void Start()
    {
        InitializeRTE(); 

        typingManager.GetComponent<TypingManager>().SetTypingEvent(
            gameObject.GetComponent<TypingEvent>()
        );
    }

    public override void Pass()
    {
        typingManager.GetComponent<TypingManager>().SetTypingEvent(
            nextTypingEventObject.GetComponent<TypingEvent>()
        );
    }

    public override void Fail() {
        typingManager.GetComponent<TypingManager>().SetTypingEvent(
            nextTypingEventObject.GetComponent<TypingEvent>()
        );
    }
}

