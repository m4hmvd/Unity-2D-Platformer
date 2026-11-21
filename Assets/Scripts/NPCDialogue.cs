using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            string[] dialogue =
            {"Red Riding Hood: .. Um, hello? What exactly are you?",
            "Bobby: I'm Bobby! I'm just a guy. Do you know a way outta here?",
            "Red Riding Hood: Shoot, I was about to ask the same thing.",
            "You came from the surface, right? I've been trying to leave here since forever .. ",
            "Folks down here say that the way out is by finding something called .. ",
            "Uhm .. 'The Eternal Dahlia', I think - or something like that.",
            "Bobby: Oh? Well .. my eyesight's absolutely terrible in this darkness.",
            "You wanna come along? Two pairs of eyes better than one and all that?",
            "Red Riding Hood: Guess so .. OK, um, lead the way.",
            };
            dialogueManager.SetSentences(dialogue);
            dialogueManager.StartCoroutine(dialogueManager.TypeDialogue());
            Destroy(GetComponent<Collider2D>(), 5f);
        }
    }
}
