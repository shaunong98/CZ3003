using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Done by Jun Hao and Zhi Fah
public class QuestionListItem : MonoBehaviour
{
    //This is the text component of the question
    public Text Question;
    // This is the TeacherFireBase item
    public TeacherFireBase item;
    //This is the Firebase object
    public GameObject Firebase;
    //This is the Text within the button
    public Text ButtonText;
    //This is the button to add the question to an assignment
    public Button Addbutton;
    // This method find the firebase object when the QuestionListItem is instantiated
    void Awake()
    {
        // Setting up the reference.
        GameObject FireBase = GameObject.Find("TeacherFireBase");
        item = FireBase.GetComponent<TeacherFireBase>();

    }
    //This method pushed the question into the QuestionListItem object
    //_Question: question to be displayed
    public void NewQuestionItem(string _Question)
    {
        Question.text = _Question;
    }
    //This method makes the add button not interactable and changes the text in the button. It is pushes the question into the teacher firebase file
    public void passData()
    {
        item.Addquestion(Question.text);
        ButtonText.text = "Added";
        Addbutton.GetComponent<Button>().interactable = false;
    }
}
