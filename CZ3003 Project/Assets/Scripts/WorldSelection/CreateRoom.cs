using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateRoom : MonoBehaviour
{
    public static CreateRoom instance;
    public Dropdown WorldSelection;
    public static int World;
    public Dropdown DifficultySelect;
    public static string Difficulty;
    public Dropdown SectionSelect;
    public static int Section;
    public TMP_InputField createRoomID;
    public static string Room;
    public GameObject SelectionPanel;
    public GameObject QuestionDisplayPanel;
    public GameObject StartPanel;
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
    public void CreateRoomButton()
    {
        Room = createRoomID.text;
        Debug.Log(Room);
        SelectionPanel.gameObject.SetActive(true);
        StartPanel.gameObject.SetActive(false);
    }
    public void PressBackButtonforSelection()
    {
        SelectionPanel.gameObject.SetActive(false);
        StartPanel.gameObject.SetActive(true);
    }
    public void PressSearchButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        SelectionPanel.gameObject.SetActive(false);
    }
    public void PressBackButton()
    {
        QuestionDisplayPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(true);
    }
    /*public void PressBackButtonForQuestion()
    {
        QuestionDisplayPanel.gameObject.SetActive(true);
        QuestionPanel.gameObject.SetActive(false);
    }*/

}
