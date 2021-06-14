using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainController : MonoBehaviour
{
    [SerializeField] private CanvasGroup textCanvas;
    [SerializeField] private GameObject beginDisplay;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject mastermindModel;
    [SerializeField] private Transform mastermindPosition;
    [SerializeField] private DialogueElement dialogueController;
    
    private Animator _animator;
    
    private float textBlinkTime = 1.0f;
    
    public bool inited = false;
    
    private void Start()
    {
        textCanvas.DOFade(0.0f, textBlinkTime).SetLoops(-1, LoopType.Yoyo);
        _animator = mastermindModel.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (Input.anyKey && !inited)
        {
            beginDisplay.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f).OnComplete(
                dialogueController.onDialogueBegin.Invoke);
            _animator.SetTrigger("BeginClick");
            StartCoroutine(LerpFromTo(mainCamera.transform.position, mastermindPosition.position, 2f) );
            mainCamera.transform.DORotate(transform.eulerAngles + Quaternion.AngleAxis(180, Vector3.up).eulerAngles, 2f);
            inited = true;
        }
    }
    
    IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration) {
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            mainCamera.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        mainCamera.transform.position = pos2;
    }
}
