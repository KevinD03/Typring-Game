using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject typingManager;
    public GameObject nextTypingEventObject;

    private Player player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (player != null)
            {
                player.isCollide = true;

                typingManager.GetComponent<TypingManager>().SetTypingEvent(
                    nextTypingEventObject.GetComponent<TypingEvent>()
                );
            }
        }
    }
}
