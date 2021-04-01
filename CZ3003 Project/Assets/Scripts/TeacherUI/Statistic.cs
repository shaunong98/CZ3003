using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistic : MonoBehaviour
{
    string StudentSearched;
    public InputField SearchBar;
    public GameObject OverviewPanel;
    public GameObject StudentPanel;
    public GameObject[] Students;
    public Text[] StudentId;

    public void SearchStudent()
    {
        StudentSearched = SearchBar.text;
        Debug.Log(StudentSearched);
        SearchBar.text = "";
        DisplayStudentStatistic();
    }
    public void DisplayStudentStatistic()
    {
        StudentPanel.gameObject.SetActive(true);
        OverviewPanel.gameObject.SetActive(false);
    }
    public void ClickonStudent(int index)
    {
        StudentSearched = StudentId[index].text.ToString();
        Debug.Log(StudentSearched);
        DisplayStudentStatistic();
    }
    public void ShowInformation()
    {
        StudentPanel.gameObject.SetActive(false);
        OverviewPanel.gameObject.SetActive(true);
    }
    public void ClickBackbutton()
    {
        StudentPanel.gameObject.SetActive(false);
        OverviewPanel.gameObject.SetActive(true);
    }
}

