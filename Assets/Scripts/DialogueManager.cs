using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    private string[] dialogueSentences;
    private int index = 0;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject dialogueBox;
    public Rigidbody2D playerRB;

    void Start()
    {
        dialogueBox.SetActive(false);
        continueButton.SetActive(false);
        // the text covers the whole panel (including the Next button), so let clicks pass through it
        textDisplay.raycastTarget = false;
    }


    void Update()
    {

    }
    public void SetSentences(string[] sentences)
    {
        this.dialogueSentences = sentences;
    }
    public IEnumerator TypeDialogue()
    {

        dialogueBox.SetActive(true);
        playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        foreach (char letter in dialogueSentences[index].ToCharArray())
        {
            textDisplay.text += letter;

            yield return new WaitForSeconds(typingSpeed);

            if (textDisplay.text == dialogueSentences[index]) { continueButton.SetActive(true); }
        }
    }

    public void NextSentence()
    {

        Debug.Log("Inside NextSentence");
        continueButton.SetActive(false);
        if (index < dialogueSentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(TypeDialogue());

        }
        else
        {
            textDisplay.text = "";
            dialogueBox.SetActive(false);
            this.dialogueSentences = null;
            index = 0;
            playerRB.constraints = RigidbodyConstraints2D.None;
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
    }
}
