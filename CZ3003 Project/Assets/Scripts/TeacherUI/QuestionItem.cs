using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Done by Jun Hao and Zhi Fah
public class QuestionItem : MonoBehaviour
{
    //This is the text component for the question
    public Text Question;
    // This is the world which the question can come from
    int World;
    // This is the section which the question can come from
    int Section;
    // This is the TeacherFireBase item
    public TeacherFireBase item;
    //This is the Firebase object
    public GameObject Firebase;
    // This method find the firebase object when the QuestionItem is instantiated
    void Awake()
    {
        // Setting up the reference.
        GameObject FireBase = GameObject.Find("TeacherFireBase");
        item = FireBase.GetComponent<TeacherFireBase>();

    }
    //This method pushed the question into the questionItem object
    //_Question: question to be displayed
    //_world: world the question came from
    //_section: section the question came from
    public void NewQuestionItem(string _Question, int _world, int _Section)
    {
        Question.text = _Question;
        World = _world;
        Section = _Section;
    }
    // This method is called when the edit button is clicked. It is pushes the question into the teacher firebase file
    public void passData()
    {
        item.EditData(Question.text);
    }
}
