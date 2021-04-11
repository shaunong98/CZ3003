using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour
{
    public GameObject AddQuestionPanel;
    public GameObject LeaderBoardPanel;
    public GameObject ViewStatisticPanel;
    public GameObject MakeAssignmentPanel;
    public GameObject FunctionSelectionPanel;
    public GameObject EditQuestionPanel;
    public GameObject ScorePanel;

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
}
