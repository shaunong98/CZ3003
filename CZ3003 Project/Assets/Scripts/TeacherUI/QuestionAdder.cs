using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAdder : MonoBehaviour
{
    public GameObject AddQuestionPanel;
    public GameObject OptionSelectionPanel;

    public void PressNextButton()
    {
        AddQuestionPanel.gameObject.SetActive(true);
        OptionSelectionPanel.gameObject.SetActive(false);
    }
    public void PressBackButton()
    {
        AddQuestionPanel.gameObject.SetActive(false);
        OptionSelectionPanel.gameObject.SetActive(true);
    }
}
