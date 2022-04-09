using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseScript : MonoBehaviour
{
    public GameObject character = null;
    public GameObject deathScreen = null;
    public CanvasGroup canvas;
    private PathFollower player;
    public float transitionTime = 0.2f;
    private bool fadingOutDeathImage = false;
    private bool transitioning = false;

    void Start()
    {
        player = character.GetComponent<PathFollower>();
    }

    // Update is called once per frame

    void Update()
    {

        if (player.currentHealth <= 0 && !transitioning)
        {
            transitioning = true;
            StartCoroutine(DelayFadeStart());
            Restart();
            Time.timeScale = 0.1f;
            deathScreen.SetActive(true);
        }

        if (fadingOutDeathImage) {
            canvas.alpha -= Time.deltaTime * 4;
        }
    }


    public void Restart()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }


    IEnumerator DelayFadeStart()
    {
        yield return new WaitForSeconds(transitionTime / 3.0f);
        fadingOutDeathImage = true;
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(transitionTime);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(levelIndex);
    }
}
