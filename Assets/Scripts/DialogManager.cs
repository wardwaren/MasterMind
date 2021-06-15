using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// make a guess -> yes -> end game
//              -> no -> made mistake -> how many bulls -> how many cows ->let me try again->
//   ^^^
public class DialogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private MainController mainController;
    [SerializeField] private GameControl gameControl;
    [SerializeField] private Animator characterAnimator;
    
    private Queue<string> sentences;
    private bool typing = false;
    private bool reading = false;
    
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        reading = true;
        
        DisplayNextSentence();
    }

    public void AddSentence(string sentence)
    {
        sentences.Enqueue(sentence);
        DisplayNextSentence();
    }
    
    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        characterAnimator.SetTrigger("StartTalking");
        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    public IEnumerator TypeSentence(string sentence, Action callback = null, float waitSeconds = 0.0f)
    {
        typing = true;
        mainText.text = "";
        
        foreach (char letter in sentence.ToCharArray())
        {
            mainText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        typing = false;

        if (callback != null)
        {
            yield return new WaitForSeconds(waitSeconds);
            callback.Invoke();
        }
    }
    
    private void EndDialogue()
    {
        gameControl.GuessQuestion();
        reading = false;
    }

    private void FixedUpdate()
    {
        if (Input.anyKey && reading && !typing)
        {
            DisplayNextSentence();
        }
    }
}
