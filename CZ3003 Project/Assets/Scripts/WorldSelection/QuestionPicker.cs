using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Done by Jun Hao and Jethro
public class QuestionPicker : MonoBehaviour
{

    //This is the question
    public Text Question;
    //This is the custom firebase object
    public CustomFirebase item;
    //This is the firebase gameobject
    public GameObject Firebase;
    //This is the button text
    public Text ButtonText;
    //This is the add button
    public Button Addbutton;
    //This method calls the custom firebase object when the object is initialised
    void Awake()
    {
        // Setting up the reference.
        GameObject FireBase = GameObject.Find("Canvas");
        item = FireBase.GetComponent<CustomFirebase>();

    }
    //This method sets the question in the Question Picker object
    //_Question: Question to be displayed
    public void NewQuestionItem(string _Question)
    {
        Question.text = _Question;
    }
    //This method is called to make the button not interactable and pass the question selected to the custom firebase
    public void passData()
    {
        item.Addquestion(Question.text);
        ButtonText.text = "Added";
        Addbutton.GetComponent<Button>().interactable = false;
    }
}
