using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

    public float transitionTime = 12f;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(LoadLevel());
    }


    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(0);
    }
}
