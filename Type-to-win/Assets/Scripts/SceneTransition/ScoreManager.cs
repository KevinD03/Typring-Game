using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text scoreText;
    public GameObject addPrefab;
    public GameObject subtractPrefab;
    public GameObject myScore;


    int score = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreText.text = score.ToString() + " POINTS";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoint()
    {
        score += 10;
        scoreText.text = score.ToString() + " POINTS";
        GameObject add = Instantiate(addPrefab, myScore.transform);
        add.transform.parent = myScore.transform;
        RectTransform subtractTransform = add.GetComponent<RectTransform>();
        subtractTransform.anchoredPosition = new Vector2(0, 0);
        Destroy(add, 3);
    }

    public void SubtractPoint()
    {
        score -= 5;
        scoreText.text = score.ToString() + " POINTS";
        GameObject subtract = Instantiate(subtractPrefab, myScore.transform);
        subtract.transform.parent = myScore.transform;
        RectTransform subtractTransform = subtract.GetComponent<RectTransform>();
        subtractTransform.anchoredPosition = new Vector2(0, 0);
        Destroy(subtract, 3);
    }
}
