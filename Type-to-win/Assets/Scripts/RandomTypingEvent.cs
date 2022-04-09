using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class RandomTypingEvent : TypingEvent
{
    // Insert the lengths via S, M, L
    // prompt time is then determined via
    // difficulty level and character length +
    // reaction time offset.
    public string[] randomPromptSeeds;

    // Loaded once to store all words by length for
    // random selection
    private static List<List<string>> dictionary;


    // Float from 0-1 to represent WPM floor and difficulty
    // for calculating how much time players have to type
    public static float difficulty = 0.4f;
    public const float maxDifficultyWPM = 100.0f;

    private const int S_LENGTH = 6;
    private const int M_LENGTH = 9;
    private const int L_LENGTH = 15;


    protected void InitializeRTE()
    {
        if (dictionary == null) {
            Debug.Log("Loading random prompt dictionary");
            dictionary = LoadWords();
        }

        typingPrompt = GeneratePrompt();
        eventTime = CalculateEventTime();
    }


    private List<List<string>> LoadWords()
    {
        TextAsset dictionaryTextFile
            = Resources.Load<TextAsset>("RandomTypingEventWords");

        List<string> smallWords = new List<string>();
        List<string> mediumWords = new List<string>();
        List<string> largeWords = new List<string>();

        string[] lines = dictionaryTextFile.text.Split('\n');
        Debug.Log(lines.Length);
        for (int i = 0; i < lines.Length; i++) {
            
            if (lines[i].StartsWith("#")) {
                continue;
            }

            lines[i] = lines[i].Trim();
            if (lines[i].Length == S_LENGTH) {
                smallWords.Add(lines[i]);
            }

            if (lines[i].Length == M_LENGTH) {
                mediumWords.Add(lines[i]);
            }

            if (lines[i].Length == L_LENGTH) {
                largeWords.Add(lines[i]);
            }
        }
        
        List<List<string>> dictionaryList = new List<List<string>>();
        dictionaryList.Add(smallWords);
        dictionaryList.Add(mediumWords);
        dictionaryList.Add(largeWords);

        return dictionaryList;
    }


    private string GeneratePrompt()
    {
        List<string> newPrompt = new List<string>();

        for (int i = 0; i < randomPromptSeeds.Length; i++) {
            string type = randomPromptSeeds[i];
            int nextWordIndex;

            if (type.Equals("s")) {
                Debug.Log(dictionary[0].Count);
                nextWordIndex = Random.Range(0, dictionary[0].Count);
                Debug.Log(nextWordIndex);
                newPrompt.Add(dictionary[0][nextWordIndex]);
            }

            if (type.Equals("m")) {
                nextWordIndex = Random.Range(0, dictionary[1].Count);
                newPrompt.Add(dictionary[1][nextWordIndex]);
            }
            
            if (type.Equals("l")) {
                nextWordIndex = Random.Range(0, dictionary[2].Count);
                newPrompt.Add(dictionary[2][nextWordIndex]);
            }
        }
        
        if (newPrompt.Count == 0) {
            return "placeholder";
        }

        return string.Join(" ", newPrompt);
    }


    private uint CalculateEventTime()
    {
        uint reactionTimeOffsetMs = 333;

        float difficultyFloorWPM = difficulty * maxDifficultyWPM;
        float charactersPerWord = 5.0f;

        float secondsPerCharacter
            = 60.0f / (difficultyFloorWPM * charactersPerWord);

        float totalPromptSeconds
            = typingPrompt.Length * secondsPerCharacter;

        uint totalPromptMs = Convert.ToUInt32(totalPromptSeconds * 1000.0f)
            + reactionTimeOffsetMs;

        return totalPromptMs;
    }
}

