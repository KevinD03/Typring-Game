using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel3 : MonoBehaviour
{
    public GameObject panel = null;
    public CanvasGroup canvas;
    public float transitionTime = 9f;

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
