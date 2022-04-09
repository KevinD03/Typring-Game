using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBGScript : MonoBehaviour
{
    public GameObject character = null;
    public CanvasGroup canvas;
    private PathFollower player;
    public float transitionTime = 0.1f;


    void Start()
    {
        player = character.GetComponent<PathFollower>();
    }


    void Update()
    {
        if (player.currentHealth <= 0)
        {
            Load();
        }
    }

    public void Load()
    {
        StartCoroutine(LoadBlack());
    }

    IEnumerator LoadBlack()
    {
        yield return new WaitForSeconds(transitionTime);
        canvas.alpha += Time.deltaTime * 10;
    }
}
