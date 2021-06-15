using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Add dialogue with currentGuess, start animation, display answer buttons ->
// if yes -> add dialogue and stop game
// if no -> add dialogue with guess and return to 1

public class GameControl : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private HashSet<string> possiblePermutations;
    private string currentGuess;
    private int currentCows;
    private int currentBulls;
    
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

    public void GuessQuestion()
    {
        
    }
    
    

    public void CorrectGuess()
    {
         
    }

    public void WrongGuess()
    {
        PruneCombinations();
        currentGuess = possiblePermutations.First();
        GuessQuestion();
    }

    private void PruneCombinations()
    {
        
    }
    
    private bool CompareBullsCows(string a, string b, int cows, int bulls)
    {
        return true;
    }
    
}
