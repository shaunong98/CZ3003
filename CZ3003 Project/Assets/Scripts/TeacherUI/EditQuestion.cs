using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditQuestion : MonoBehaviour
{
    // The instance of the file that can be reference by other files later
    public static EditQuestion instance;
    //The Dropdown for selecting the world when editing questions
    public Dropdown WorldSelection;
    // Variable to store the world that the user has selected
    public static int World;
    //The Dropdown for selecting the difficulty when editing questions
    public Dropdown DifficultySelect;
    // Variable to store the difficulty that the user has selected
    public static string Difficulty;
    //The Dropdown for selecting the section when editing questions
    public Dropdown SectionSelect;
    // Variable to store the section that the user has selected
    public static int Section;
    // The panel where the user can select the filters of world, section and difficulty 
    public GameObject SelectionPanel;
    // The panel where the question are all displayed after being filtered
    public GameObject QuestionDisplayPanel;
    // The panel where 
    public GameObject QuestionPanel;
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
    public void WorldSelect()
    {
        Debug.Log(WorldSelection.options[WorldSelection.value].text);
        switch (WorldSelection.options[WorldSelection.value].text)
        {
            case "OODP":
                World = 1;
                break;
            case "SE":
                World = 2;
                break;
            case "SSAD":
                World = 3;
                break;
            default:
                Debug.Log("Error Occured");
                break;
        }
        Debug.Log(World);
    }
    public void DifficultySelection()
    {
        Difficulty = DifficultySelect.options[DifficultySelect.value].text;
        Debug.Log(Difficulty);
    }
    public void SectionSelection()
    {
        Section = int.Parse(SectionSelect.options[SectionSelect.value].text);
        Debug.Log(Section);
    }

    public void PressNextButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        SelectionPanel.gameObject.SetActive(false);
    }
    public void PressBackButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(true);
    }
    public void PressBackButtonForQuestion()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        QuestionPanel.gameObject.SetActive(false);
    }

}
