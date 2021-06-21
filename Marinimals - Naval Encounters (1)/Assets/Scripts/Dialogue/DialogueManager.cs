using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();

    private string currentSentence;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        GameManager.instance.inDialogue = animator.GetBool("isOpen");

        if (animator.GetBool("isOpen") && Input.GetButtonDown(GameManager.instance.anyPlayer.aButton))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        currentSentence = "";
        dialogueText.text = "";

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueText.text == currentSentence)
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            currentSentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
        }
        else if (dialogueText.text != currentSentence)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }
}
