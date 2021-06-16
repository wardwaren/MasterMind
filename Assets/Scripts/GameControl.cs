using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

// Add dialogue with currentGuess, start animation, display answer buttons ->
// if yes -> add dialogue and stop game
// if no -> add dialogue with guess and return to 1

public class GameControl : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private CanvasGroup yesButton;
    [SerializeField] private CanvasGroup noButton;
    [SerializeField] private Animator characterAnimator;
    
    private HashSet<string> possiblePermutations;
    private KeyCode[] keyCodes = {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
    };

    private string currentGuess;
    private int currentCows;
    private int currentBulls;
    private bool listeningForCows = false;
    private bool listeningForBulls = false;
    private static readonly int StartThinking = Animator.StringToHash("StartThinking");

    private void Start()
    {
        possiblePermutations = new HashSet<string>();
        
        for (var i = '0'; i <= '9'; i++)
        {
            for (var j = '0'; j <= '9'; j++)
            {
                if (i == j) continue;
                for (var k = '0'; k <= '9'; k++)
                {
                    if (k == i || k == j) continue;
                    for (var l = '0'; l <= '9'; l++)
                    {
                        if (l == i || l == j || l == k) continue;
                        string currComb = "";
                        currComb += i;
                        currComb += j;
                        currComb += k;
                        currComb += l;
                        possiblePermutations.Add(currComb);
                    }
                }
            }
        }
        
        currentGuess = possiblePermutations.First();
        currentCows = 0;
        currentBulls = 0;
    }

    private void Update()
    {
        if (listeningForCows)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    currentCows = i;
                    AskForBulls();
                }
            }
        }

        if (listeningForBulls)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    currentBulls = i;
                    GuessAgain();
                }
            }
        }
    }

    public void GuessQuestion()
    {
        if (possiblePermutations.Count == 0)
        {
            characterAnimator.SetTrigger("FinishGame");
            string confusedSentence = "Unfortunately there are no possible passwords left";
            StartCoroutine(dialogManager.TypeSentence(confusedSentence,ReloadScene, 1.0f));
            return;
        }
        string introSentence = "Is your password " + currentGuess + "?";
        StartCoroutine(dialogManager.TypeSentence(introSentence,(() => ShowButtons(true)), 1.0f));
    }
    
    public void CorrectGuess()
    {
        characterAnimator.SetTrigger("FinishGame");
        ShowButtons(false);
        string winningSentence = "Great! let's repeat this exercise again!";
        StopAllCoroutines();
        StartCoroutine(dialogManager.TypeSentence(winningSentence, ReloadScene, 1.0f));
    }

    // Ask for cows -> ask for bulls -> prune -> ask question
    public void WrongGuess()
    {
        characterAnimator.SetTrigger("StartThinking");
        ShowButtons(false);
        string wrongSentence = "Hmm... let me ask you some questions then";
        
        StopAllCoroutines();
        StartCoroutine(dialogManager.TypeSentence(wrongSentence, AskForCows, 1.0f));
    }

    private void AskForCows()
    {
        string cowsSentence = "How many digits are in the right position now?";
        
        StopAllCoroutines();
        StartCoroutine(dialogManager.TypeSentence(cowsSentence,(() => listeningForCows = true)));
    }
    
    private void AskForBulls()
    {
        listeningForCows = false;
        string bullsSentence = "How many digits are correct but in the wrong position?";
        
        StopAllCoroutines();
        StartCoroutine(dialogManager.TypeSentence(bullsSentence, () => listeningForBulls = true));
    }

    private void GuessAgain()
    {
        listeningForBulls = false;
        PruneCombinations();
        
        currentGuess = possiblePermutations.First();
        string losingSentence = "Let me try another number";
        
        StopAllCoroutines();
        StartCoroutine(dialogManager.TypeSentence(losingSentence, GuessQuestion, 1.0f));
    }
    
    private void PruneCombinations()
    {
        foreach (var permutation in possiblePermutations)
        {
            if (!CompareBullsCows(currentGuess, permutation, currentCows, currentBulls))
            {
                possiblePermutations.Remove(permutation);
            }
        }
    }

    private void ShowButtons(bool val)
    {
        if (val)
        {
            yesButton.DOFade(1.0f, 2.0f);
            noButton.DOFade(1.0f, 2.0f);
            yesButton.interactable = true;
            noButton.interactable = true;
        }
        else
        {
            yesButton.DOFade(0.0f, 2.0f);
            noButton.DOFade(0.0f, 2.0f);
            yesButton.interactable = false;
            noButton.interactable = false;
        }
    }
    
    private bool CompareBullsCows(string a, string b, int cows, int bulls)
    {
        return true;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
