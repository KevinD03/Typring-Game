using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class ProceduralTypingEvent : TypingEvent
{
    public string[] promptOptions;

    // Float from 0-1 to represent WPM floor and difficulty
    // for calculating how much time players have to type
    public static float difficulty = 0.40f;
    public const float maxDifficultyWPM = 100.0f;


    protected void InitializePTE()
    {
        typingPrompt = GeneratePrompt();
        eventTime = CalculateEventTime();
    }


    private string GeneratePrompt()
    {
        int promptIndex = Random.Range(0, promptOptions.Length);
        return promptOptions[promptIndex].Trim();
    }


    private uint CalculateEventTime()
    {
        uint reactionTimeOffsetMs = 500;

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

