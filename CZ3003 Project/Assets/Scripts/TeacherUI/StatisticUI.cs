using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Done by Jun Hao and Zhi Fah
public class StatisticUI : MonoBehaviour
{
    //This is an name of the user searched
    public static string studentSearched;
    // This is the searchbar
    public InputField SearchBar;
    //This panel displays the page with statistic for all users 
    public GameObject OverviewPanel;
    //This panel displays the statistics for 1 user
    public GameObject StudentPanel;

    //This method is called when a student is searched
    public void DisplayStudentStatistic()
    {
        OverviewPanel.gameObject.SetActive(false);
        StudentPanel.gameObject.SetActive(true);
    }
    //This method is called when the user clicks the back button is the student panel
    public void ClickBackbutton()
    {
        StudentPanel.gameObject.SetActive(false);
        OverviewPanel.gameObject.SetActive(true);
    }

}
