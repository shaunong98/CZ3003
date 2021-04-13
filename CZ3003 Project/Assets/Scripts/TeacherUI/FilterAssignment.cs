using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterAssignment : MonoBehaviour
{
    // The instance of the file that can be reference by other files later
    public static CreateRoom instance;
    //The Dropdown for selecting the world when adding questions to an assignment
    public Dropdown WorldSelection;
    // Variable to store the world that the user has selected
    public static int World;
    //The Dropdown for selecting the difficulty when adding questions to an assignment
    public Dropdown DifficultySelect;
    // Variable to store the difficulty that the user has selected
    public static string Difficulty;
    //The Dropdown for selecting the section when adding questions to an assignment
    public Dropdown SectionSelect;
    // Variable to store the section that the user has selected
    public static int Section;
    // The panel where the user can select the filters of world, section and difficulty 
    public GameObject SelectionPanel;
    // The panel where the question are all displayed after being filtered, these questions can be selected to be added to assignment 
    public GameObject QuestionDisplayPanel;
    // The panel where the user can either create room or view room scores
    public GameObject StartPanel;
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
    // This method returns the user back to the create room panel after he/she is done adding questions to the assignment
    public void FinishButton()
    {
        StartPanel.gameObject.SetActive(true);
        QuestionDisplayPanel.gameObject.SetActive(false);
    }
    // The method returns the user back to the create room panel when he/she clicks the back button is the filter panel
    public void PressBackButtonforSelection()
    {
        SelectionPanel.gameObject.SetActive(false);
        StartPanel.gameObject.SetActive(true);
    }
    //This method will bring the user to the screen to select the questions to add in the assignment after he/she is done with their filter
    public void PressSearchButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        SelectionPanel.gameObject.SetActive(false);
    }
    // This button brings the user back to the page to filter the world, section and difficulty
    public void PressBackButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(true);
    }
}
