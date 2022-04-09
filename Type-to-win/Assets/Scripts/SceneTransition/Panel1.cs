using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel1 : MonoBehaviour
{
    public GameObject panel = null;
    public CanvasGroup canvas;
    public float transitionTime = 3f;

    // Update is called once per frame
    void Update()
    {
        LoadNextPanel();
    }


    public void LoadNextPanel()
    {
        StartCoroutine(LoadPanel());

    }

    IEnumerator LoadPanel()
    {
        yield return new WaitForSeconds(transitionTime);
        canvas.alpha += Time.deltaTime;
    }
}
