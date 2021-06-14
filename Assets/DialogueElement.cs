using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DialogueElement : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private GameObject dialoguePanel;

    public Action onDialogueBegin;

    private void Start()
    {
        onDialogueBegin += TriggerDialogue;
    }

    private void TriggerDialogue()
    {
        dialoguePanel.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f);
        dialogManager.StartDialogue(dialogue);
    }
    
}
