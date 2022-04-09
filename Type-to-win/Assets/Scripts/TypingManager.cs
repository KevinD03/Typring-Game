using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void TypingEventCallback();

public class TypingManager : MonoBehaviour
{
    public AudioSource keyboardAudioSource;
    public AudioClip[] keyboardTypeSFX;
    public AudioClip[] keyboardFailSFX;
    public AudioClip promptPassSFX;
    public AudioClip promptFailSFX;

    //animator
    public bool level_indicator;
    public ParticleSystem muzzle;
    private Animator FPSChara_Gun_animator;
    private Animator FPSChara_Arm_animator;
    public GameObject FPSChara_Gun;
    public GameObject FPSChara_Arm;


    public TMP_Text textComponent;

    public GameObject timerPanelComponent;
    public float timerScaleUnitPerChar = 0.05f;

    public Color timerPanelStartColour;
    public Color timerPanelTimeoutColour;

    public bool debugMode;

    private TypingEvent currentTypingEvent;

    // Tracks position of user input in word as typing occurs.
    private int typingCaret = -1;
    private string currentPrompt;
    private float currentTimerPanelMaxScale;
    private float eventTimeRemaining;
    private bool playerMadeTypo = false;

    void Start() {

        if (level_indicator)
        {
            FPSChara_Gun_animator = FPSChara_Gun.GetComponent<Animator>();
            FPSChara_Arm_animator = FPSChara_Arm.GetComponent<Animator>();
        }
    }

    void Update()
    {

        // Fail when timing out during a timed prompt, if one is set
        if (currentTypingEvent != null && currentTypingEvent.eventTime != 0) {
            Vector3 newTextCanvasPosition
                = currentTypingEvent.positionObject.transform.position;

            textComponent.transform.parent.position = newTextCanvasPosition;

            eventTimeRemaining -= Time.deltaTime;

            // Update event time indicator panel
            float totalEventTime = (currentTypingEvent.eventTime / 1000.000f);
            float percentTimeDecrease = Time.deltaTime / totalEventTime;

            Vector3 panelScaleDecrement = new Vector3(
                percentTimeDecrease * currentTimerPanelMaxScale,
                0.0f,
                0.0f
            );

            timerPanelComponent.transform.localScale -= panelScaleDecrement;

            timerPanelComponent.GetComponent<Image>().color = Color.Lerp(
                timerPanelStartColour,
                timerPanelTimeoutColour,
                1.0f - (eventTimeRemaining / totalEventTime)
            );

            if (eventTimeRemaining <= 0.000f) {
                Fail();
                return;
            }
        }

        string playerInput = Input.inputString;
        if (typingCaret < 0 || !Input.anyKeyDown || playerInput.Length < 1) {
            return;
        }

        // Check latest keypresses and match to word; update caret
        for (int i = 0; i < playerInput.Length && typingCaret < currentPrompt.Length; i++) {

            if (debugMode) {
                if (playerInput[i] == '1') {
                    Pass();

                    return;
                } else if (playerInput[i] == '2') {
                    Fail();

                    return;
                }
            }

            if (playerInput[i] == currentPrompt[typingCaret]) {
                playerMadeTypo = false;
                ScoreManager.instance.AddPoint();

                typingCaret++;
            } else {
                playerMadeTypo = true;
                ScoreManager.instance.SubtractPoint();

                break;
            }
        }

        // Re-colour and render the typing prompt UI
        UpdateTextMesh();

        // Pass if the prompt is complete
        if (typingCaret == currentPrompt.Length) {
            Pass();
        }
    }

    // Set up new typing event to start on next frame after calling
    public void SetTypingEvent(TypingEvent typingEvent) {
        currentTypingEvent = typingEvent;
        currentPrompt = typingEvent.typingPrompt;
        // Replace with whitespace representative
        textComponent.text = typingEvent.typingPrompt.Replace(' ', '\u00b7');
        typingCaret = 0;
        playerMadeTypo = false;
        eventTimeRemaining = typingEvent.eventTime / 1000.000f;

        Vector3 newTextCanvasPosition
            = typingEvent.positionObject.transform.position;

        textComponent.transform.parent.position = newTextCanvasPosition;

        if (typingEvent.eventTime != 0) {
            currentTimerPanelMaxScale
                = typingEvent.typingPrompt.Length * timerScaleUnitPerChar;

            Vector3 newTimerPanelScale
                = timerPanelComponent.transform.localScale;
            newTimerPanelScale.x = currentTimerPanelMaxScale;
            timerPanelComponent.transform.localScale = newTimerPanelScale;

            timerPanelComponent.SetActive(true);
        }
    }

    // Clean up expired typing event when calling Pass() or Fail()
    private void ResetTypingEvent() {
        currentTypingEvent = null;
        currentPrompt = null;
        textComponent.text = "";
        typingCaret = -1;
        playerMadeTypo = false;
        eventTimeRemaining = 0f;
        timerPanelComponent.GetComponent<Image>().color
            = new Color32(255, 255, 225, 255);
        timerPanelComponent.SetActive(false);
    }

    private void UpdateTextMesh () {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        // Stage colour changes based on correctness and position in word.
        for (int i = 0; i < textInfo.characterCount; i++) {
            var charInfo = textInfo.characterInfo[i];

            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            for (int j = 0; j < 4; j++) {
                var index = charInfo.vertexIndex + j;

                if (i < typingCaret) {
                    meshInfo.colors32[index] = Color.green;
                    PlayTypeSound();
                } else if (i == typingCaret && playerMadeTypo) {
                    meshInfo.colors32[index] = Color.red;
                    PlayFailSound();
                }
            }
        }

        // Commit changes to visible text mesh.
        for (int i = 0; i < textInfo.meshInfo.Length; i++) {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            meshInfo.mesh.colors32 = meshInfo.colors32;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    private void Pass()
    {
        if (level_indicator)
        {
            FPSChara_Gun_animator.SetTrigger("isShootGun");
            FPSChara_Arm_animator.SetTrigger("isShootChara");
            muzzle.Play();
        }
        keyboardAudioSource.clip = promptPassSFX;
        keyboardAudioSource.Play();
        TypingEventCallback pass = currentTypingEvent.Pass;
        ResetTypingEvent();
        pass();
    }

    private void Fail()
    {
        keyboardAudioSource.clip = promptFailSFX;
        keyboardAudioSource.Play();
        TypingEventCallback fail = currentTypingEvent.Fail;
        ResetTypingEvent();
        fail();

    }

    private void PlayTypeSound()
    {
         int index = Random.Range(0, keyboardTypeSFX.Length);
         keyboardAudioSource.clip = keyboardTypeSFX[index];
         keyboardAudioSource.Play();
    }

    private void PlayFailSound()
    {
         int index = Random.Range(0, keyboardFailSFX.Length);
         keyboardAudioSource.clip = keyboardFailSFX[index];
         keyboardAudioSource.Play();
    }
}
