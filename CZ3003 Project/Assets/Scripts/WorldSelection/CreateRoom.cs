using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Done by Jun Hao and Jethro
public class CreateRoom : MonoBehaviour
{
    //This is the instance of CreateRoom
    public static CreateRoom instance;
    //This is the drop down for selecting the world
    public Dropdown WorldSelection;
    //This variable stores the selected world
    public static int World;
    //This is the drop down for selecting the difficulty
    public Dropdown DifficultySelect;
    //This variable stores the selected difficulty
    public static string Difficulty;
    //This is the drop down for selecting the section
    public Dropdown SectionSelect;
    //This variable stores the selected section
    public static int Section;
    //This inputfield contains the room the user want to create
    public TMP_InputField createRoomID;
    //This variable store the room created
    public static string Room;
    //This panel contains the filters world, section and difficulty to get the relevant questions
    public GameObject SelectionPanel;
    //This is the panel which shows all the filtered questions
    public GameObject QuestionDisplayPanel;
    //This panel contains create or play room
    public GameObject StartPanel;
    //This is the customfirebase object
    public CustomFirebase item;
    //This is the firebase object
    public GameObject Firebase;
    //This is the error message
    public Text errormsg;
    //This variable store the value of whether the room exists
    bool roomExist;

    //This method finds the instance of custom firebase
    private void Awake()
    {

        // Setting up the reference.
        GameObject FireBase = GameObject.Find("Canvas");
        item = FireBase.GetComponent<CustomFirebase>();
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
    //This method is called when the user change the world dropdown
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
    //This method is called when the difficulty dropdown is changed
    public void DifficultySelection()
    {
        Difficulty = DifficultySelect.options[DifficultySelect.value].text;
    }
    //This method is called when the section dropdown is changed
    public void SectionSelection()
    {
        Section = int.Parse(SectionSelect.options[SectionSelect.value].text);
    }
    //This method is called when the finish button is clicked
    public void FinishButton()
    {
        StartPanel.gameObject.SetActive(true);
        QuestionDisplayPanel.gameObject.SetActive(false);
    }
    //This method is called when the user clicks on the back button on the selection panel
    public void PressBackButtonforSelection()
    {
        SelectionPanel.gameObject.SetActive(false);
        StartPanel.gameObject.SetActive(true);
    }
    //This method is called when the user click the search button
    public void PressSearchButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        SelectionPanel.gameObject.SetActive(false);
    }
    //This method is called when the user clicks the back button on the question display panel
    public void PressBackButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(true);
    }


}
