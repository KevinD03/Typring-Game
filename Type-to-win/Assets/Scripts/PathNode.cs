using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public Transform GetTransform()
    {
        return transform;
    }

    // Callback when PathFollower reaches this point of the path,
    // use by overriding in child class.
    public virtual void OnArrival()
    {
        return;
    }

    public virtual void startNextTypingEvent(){
        return;
    }
}

