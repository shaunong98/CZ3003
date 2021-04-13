using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Done by Jun Hao and Zhi Fah
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
    // The panel where the question are all displayed after being filtered, these questions can be selected to be edited 
    public GameObject QuestionDisplayPanel;
    // The panel which displays the individual question to be edited after it is selected
    public GameObject QuestionPanel;
    // This method ensures there is only 1 instance of the selection panel
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
    // This methods extracts the data in the dropdown for world
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
    }
    // This methods extracts the data in the dropdown for difficulty
    public void DifficultySelection()
    {
        Difficulty = DifficultySelect.options[DifficultySelect.value].text;
    }
    // This methods extracts the data in the dropdown for section
    public void SectionSelection()
    {
        Section = int.Parse(SectionSelect.options[SectionSelect.value].text);
    }
    // This methods is called to transition from the selection panel to the display all question panel after the next button is pressed on the selection panel
    public void PressNextButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        SelectionPanel.gameObject.SetActive(false);
    }
    // This methods is called to transition from the display all question panel to the selection panel after the back button is pressed on the display all questions panel
    public void PressBackButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(true);
    }
    // This methods is called to transition from the individual question panel to the display all question panel after the back button is pressed on the individual questions panel
    public void PressBackButtonForQuestion()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        QuestionPanel.gameObject.SetActive(false);
    }

}
