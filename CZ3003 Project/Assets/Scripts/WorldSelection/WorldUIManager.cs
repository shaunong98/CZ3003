using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager instance;
    public GameObject mapSelectionPanel;
    public GameObject[] levelSelectionPanels;
    public GameObject modeSelectionPanel;
    public GameObject PVPSelectionPanel;

    public int stars;
    public Text startText;
    public WorldSelection[] WorldSelections;
    public Text[] questStarsText;
    public Text[] lockedStarsText;
    public Text[] unlockedStarsText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        //DontDestroyOnLoad(gameObject);

    }
    private void Start()
    {

        int temp = PlayerPrefs.GetInt("CharacterSelected");
        //PlayerPrefs.DeleteAll();   //delete all player key and values from player prefs
        PlayerPrefs.SetInt("CharacterSelected",temp);
    }
    private void Update()
    {
        UpdateUnlockedStar();
        UpdateStars();
    }
    private void UpdateUnlockedStar()
    {
        for (int i=0; i<WorldSelections.Length;i++)
        {
            if (WorldSelections[i].isUnlock == true)
            {
                if (stars - (WorldSelections[i].startLevel-1)*4>=0 && stars< WorldSelections[i].endLevel * 4)
                    unlockedStarsText[i].text = (stars- (WorldSelections[i].startLevel-1)*4).ToString() + "/" + ((WorldSelections[i].endLevel-WorldSelections[i].startLevel)+1) * 4;
                else if (stars >= WorldSelections[i].endLevel * 4)
                    unlockedStarsText[i].text = ((WorldSelections[i].endLevel - WorldSelections[i].startLevel + 1) * 4).ToString()+"/" + (WorldSelections[i].endLevel - WorldSelections[i].startLevel + 1) * 4;
                else
                    unlockedStarsText[i].text =  "0/" + ((WorldSelections[i].endLevel - WorldSelections[i].startLevel)+1) * 4;
            }
        }
    }
    private void UpdateStars()
    {
        stars = PlayerPrefs.GetInt("Lv" + 1) + PlayerPrefs.GetInt("Lv" + 2) + PlayerPrefs.GetInt("Lv" + 3) + PlayerPrefs.GetInt("Lv" + 4) + PlayerPrefs.GetInt("Lv" + 5) + PlayerPrefs.GetInt("Lv" + 6) + PlayerPrefs.GetInt("Lv" + 7) + PlayerPrefs.GetInt("Lv" + 8) + PlayerPrefs.GetInt("Lv" + 9) + PlayerPrefs.GetInt("Lv" + 10) + PlayerPrefs.GetInt("Lv" + 11) + PlayerPrefs.GetInt("Lv" + 12) + PlayerPrefs.GetInt("Lv" + 13) + PlayerPrefs.GetInt("Lv" + 14) + PlayerPrefs.GetInt("Lv" + 15) + PlayerPrefs.GetInt("Lv" + 16);
        startText.text = stars.ToString();
    }
    public void PressMapButton(int _mapIndex)
    {
        levelSelectionPanels[_mapIndex].gameObject.SetActive(true);
        mapSelectionPanel.gameObject.SetActive(false);
    }
    public void PressSectionBackButton()
    {
        mapSelectionPanel.gameObject.SetActive(true);
        for (int i=0; i< levelSelectionPanels.Length;i++)
            levelSelectionPanels[i].gameObject.SetActive(false);
    }
    public void SceneTransition(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void PressQuestModeButton()
    {
        mapSelectionPanel.gameObject.SetActive(true);
        modeSelectionPanel.gameObject.SetActive(false);
    }
    public void PressQuestBackButton()
    {
        mapSelectionPanel.gameObject.SetActive(false);
        modeSelectionPanel.gameObject.SetActive(true);
    }
    public void PressPVPModeButton()
    {
        PVPSelectionPanel.gameObject.SetActive(true);
        modeSelectionPanel.gameObject.SetActive(false);
    }
    public void PressPVPBackButton()
    {
        PVPSelectionPanel.gameObject.SetActive(false);
        modeSelectionPanel.gameObject.SetActive(true);
    }
    public void BackLevelSelection()
    {
        SceneManager.LoadScene("Map Selection");
    }

    public void gameStart()
    {
        for (int i=0; i< levelSelectionPanels.Length;i++)
            levelSelectionPanels[i].gameObject.SetActive(false);
    }

    public void setAllToFalse() {
        mapSelectionPanel.gameObject.SetActive(false);
        for (int i=0; i< levelSelectionPanels.Length;i++)
            levelSelectionPanels[i].gameObject.SetActive(false);
        modeSelectionPanel.gameObject.SetActive(false);
        PVPSelectionPanel.gameObject.SetActive(false);
    }

    public void Openforum()
    {
        Application.OpenURL("https://softwaremon.quora.com/");
    }

}
