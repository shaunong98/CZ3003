using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPicker : MonoBehaviour
{
    public Text Question;
    //int World;
    //int Section;
    public CustomFirebase item;
    public GameObject Firebase;
    public Text ButtonText;
    public Button Addbutton;

    void Awake()
    {
        // Setting up the reference.
        GameObject FireBase = GameObject.Find("Canvas");
        item = FireBase.GetComponent<CustomFirebase>();

    }
    public void NewQuestionItem(string _Question)
    {
        Question.text = _Question;
    }
    public void passData()
    {
        item.Addquestion(Question.text);
        ButtonText.text = "Added";
        Addbutton.GetComponent<Button>().interactable = false;
    }
}
