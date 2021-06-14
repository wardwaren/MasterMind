using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private MainController mainController;
    
    private Queue<string> sentences;
    private bool typing = false;
    
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

        DisplayNextSentence();
    }
    
    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        typing = true;
        mainText.text = "";
        
        foreach (char letter in sentence.ToCharArray())
        {
            mainText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        typing = false;
    }
    
    private void EndDialogue()
    {
            
    }

    private void FixedUpdate()
    {
        if (Input.anyKey && mainController.inited && !typing)
        {
            DisplayNextSentence();
        }
    }
}
