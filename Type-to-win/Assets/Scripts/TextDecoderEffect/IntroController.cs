using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public Player Player;
    public CanvasGroup intro;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (Player.startMoving == true) {
            intro.alpha -= Time.deltaTime;
        }
        
    }
}
