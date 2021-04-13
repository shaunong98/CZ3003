using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour
{
    // This is an instance of UIcontroller
    public static UIcontroller instance;
    public GameObject AddQuestionPanel;
    public GameObject LeaderBoardPanel;
    public GameObject ViewStatisticPanel;
    public GameObject MakeAssignmentPanel;
    public GameObject FunctionSelectionPanel;
    public GameObject EditQuestionPanel;
    public GameObject ScorePanel;

    public Dropdown WorldLeaderboard;
    public static int WorldLdrboard;
    public Dropdown SectionLeaderboard;
    public static int SectionLdrboard;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void PressAddQuestionButton()
    {
        AddQuestionPanel.gameObject.SetActive(true);
        FunctionSelectionPanel.gameObject.SetActive(false);
    }
    public void PressLeaderboardButton()
    {
        LeaderBoardPanel.gameObject.SetActive(true);
        FunctionSelectionPanel.gameObject.SetActive(false);
    }
    public void PressViewStatisticButton()
    {
        ViewStatisticPanel.gameObject.SetActive(true);
        FunctionSelectionPanel.gameObject.SetActive(false);
    }
    public void PressMakeAssignmentButton()
    {
        MakeAssignmentPanel.gameObject.SetActive(true);
        FunctionSelectionPanel.gameObject.SetActive(false);
    }
    public void PressEditQuestionButton()
    {
        EditQuestionPanel.gameObject.SetActive(true);
        FunctionSelectionPanel.gameObject.SetActive(false);
    }
    public void PressBackAddQuestionButton()
    {
        AddQuestionPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);
    }
    public void PressBackLeaderboardButton()
    {
        LeaderBoardPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);
    }
    public void PressBackViewStatisticButton()
    {
        ViewStatisticPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);
    }
    public void PressBackMakeAssignmentButton()
    {
        MakeAssignmentPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);
    }
    public void PressBackEditQuestionButton()
    {
        EditQuestionPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);
    }
    public void Presslogoutbutton()
    {
        SceneManager.LoadScene("LoginScene");
    }
    public void PressBackScorePanel()
    {
        ScorePanel.gameObject.SetActive(false);
        MakeAssignmentPanel.gameObject.SetActive(true);
    }

    public void WorldLdrboardSelect()
    {
        Debug.Log(WorldLeaderboard.options[WorldLeaderboard.value].text);
        switch (WorldLeaderboard.options[WorldLeaderboard.value].text)
        {
            case "OODP":
                WorldLdrboard = 1;
                break;
            case "SE":
                WorldLdrboard = 2;
                break;
            case "SSAD":
                WorldLdrboard = 3;
                break;
            default:
                Debug.Log("Error Occured");
                break;
        }
        Debug.Log(WorldLdrboard);
    }

    public void SectionLdrboardSelection()
    {
        SectionLdrboard = int.Parse(SectionLeaderboard.options[SectionLeaderboard.value].text);
        Debug.Log(SectionLdrboard);
    }
}
