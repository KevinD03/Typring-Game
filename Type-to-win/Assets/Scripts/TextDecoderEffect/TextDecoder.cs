using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextDecoder : MonoBehaviour
{
    [Tooltip("The time between character change")]
    public float randomizeDelay = 0.05f;

    [Tooltip("The time until the text begins to reveal")]
    public float textShowDelay = 3f;

    [Tooltip("Force the full text to show after elapsed time")]
    public bool forceText = false;

    [Tooltip("Elapsed time until the text will force show (Counts after ShowDelay)")]
    public float forceTime = 2;

    [Tooltip("The text to reveal")]
    public string textToWrite;

    [Tooltip("The textbox to write to")]
    private Text textBox;

    private bool reveal = false;
    private char[] letters = " QWERTYUIOPASDFGHJKLZXCVBNM1234567890 ".ToCharArray();
    private string randomText = "";

    private void Awake()
    {
        textBox = GetComponent<Text>();
    }

    private void Start()
    {
        StartCoroutine(TextDecode());
    }


    // Call this coroutine to begin text decoder
    private IEnumerator TextDecode()
    {
        PopulateRandomText(); // Populates the random letter string with same length as text to write
        InvokeRepeating("RandomizeLetters", 0f, randomizeDelay); // Start randomizing text
        
        // Start revealing letters after elapsed time
        yield return new WaitForSeconds(textShowDelay);
        reveal = true; // Reveal 

        // Force Text to show after specified seconds (Counts after show delay)
        if(forceText == true & textBox.text != textToWrite)
        {
            yield return new WaitForSeconds(forceTime);
            Debug.Log("force");
            CancelInvoke();
            textBox.text = textToWrite;
        }

        yield return null;
    }


    // Randomizes the letters in the string
    public void RandomizeLetters()
    {
        // Loops through each char in the text to write
        for (int i = 0; i < textToWrite.Length; i++)
        {
            // If reveal is true then start to skip letters (resulting in the full string being shown)
            if (reveal == true)
            {
                // Skip letter
                if (randomText[i].ToString() == textToWrite[i].ToString().ToUpper())
                {
                    continue;
                }
            }

            int randomLetterGen = Random.Range(0, letters.Length); // Picks random character in the generator

            randomText = randomText.Remove(i, 1); // Remove character from position
            randomText = randomText.Insert(i, letters[randomLetterGen].ToString()); // Insert new random char

            textBox.text = randomText; // Set text

            // Cancels the repeating method if the text is the same
            if(textBox.text == textToWrite)
            {
                CancelInvoke();
            }
        }
    }


    // Fills the random string with same char count as the text to write
    public void PopulateRandomText()
    {
        for (int i = 0; i < textToWrite.Length; i++)
        {
            // Return if the count length is the same (And chat count is more than 0)
            if(i > 0 && randomText.Length == textToWrite.Length)
            {
                return;
            }

            // Populate string
            randomText = randomText.Insert(i, " ");
        }
    }
}
