// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSec;

    public static DialogManager Instance{get;private set;}

    public event Action OnShowDialog;
    public event Action OnCloseDialog;
    public bool IsShowing{get;private set;}
    
    Dialog dialog;

    int currentLine = 0;
    bool isTyping;
    Action onDialogFinished;

    private void Awake() 
    {
        Instance=this;    
    }
    public void HandleUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            ++currentLine;
            if(currentLine<dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                IsShowing = false;
                dialogBox.SetActive(false);
                onDialogFinished?.Invoke();
                OnCloseDialog?.Invoke();
            }
        }
    } 

    public IEnumerator ShowDialog(Dialog dialog, Action onFinished=null)
    {
        yield return new WaitForEndOfFrame();
        IsShowing=true;
        OnShowDialog?.Invoke();
        this.dialog = dialog;
        onDialogFinished = onFinished;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
        Debug.Log("How many times");
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true; // to ensure that the dialog box will finish displaying first line even if player enters spacebar
        dialogText.text = "";
        foreach(var letter in line.ToCharArray())
        {
            dialogText.text+=letter;
            yield return new WaitForSeconds(1f/lettersPerSec);
        }
        isTyping = false;
    }
}
