using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionItem : MonoBehaviour
{
    public Text Question;
    int World;
    int Section;
    public TeacherFireBase item;
    public GameObject Firebase;

    void Awake()
    {
        // Setting up the reference.
        GameObject FireBase = GameObject.Find("TeacherFireBase");
        item = FireBase.GetComponent<TeacherFireBase>();

    }
    public void NewQuestionItem(string _Question, int _world, int _Section)
    {
        Question.text = _Question;
        World = _world;
        Section = _Section;
    }
    public void passData()
    {
        item.GetData(Question.text);
    }
}
