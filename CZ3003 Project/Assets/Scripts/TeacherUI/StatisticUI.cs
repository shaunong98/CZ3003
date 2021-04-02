using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticUI : MonoBehaviour
{
    public static string studentSearched;
    public InputField SearchBar;
    public GameObject OverviewPanel;
    public GameObject StudentPanel;
    public GameObject[] Students;
    public Text[] StudentId;

    public void DisplayStudentStatistic()
    {
        Debug.Log("Reached");
        OverviewPanel.gameObject.SetActive(false);
        StudentPanel.gameObject.SetActive(true);
    }

    public void ClickBackbutton()
    {
        StudentPanel.gameObject.SetActive(false);
        OverviewPanel.gameObject.SetActive(true);
    }

}
