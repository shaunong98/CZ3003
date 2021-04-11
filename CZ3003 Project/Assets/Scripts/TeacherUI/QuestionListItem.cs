using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionListItem : MonoBehaviour
{
    public Text Question;
    //int World;
    //int Section;
    public TeacherFireBase item;
    public GameObject Firebase;
    public Text ButtonText;
    public Button Addbutton;

    void Awake()
    {
        // Setting up the reference.
        GameObject FireBase = GameObject.Find("TeacherFireBase");
        item = FireBase.GetComponent<TeacherFireBase>();

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
