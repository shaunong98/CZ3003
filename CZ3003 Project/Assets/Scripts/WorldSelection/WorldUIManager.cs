// Authors: Jethro, Jun Hao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class WorldUIManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip cfmClickSFX;
    public static WorldUIManager instance;
    public GameObject mapSelectionPanel;
    public GameObject[] levelSelectionPanels;
    public GameObject modeSelectionPanel;
    public GameObject PVPSelectionPanel;
    public GameObject CustomSelection;

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
        int totalStars1 = PlayerPrefs.GetInt("Lv" + 1) + PlayerPrefs.GetInt("Lv" + 2) + PlayerPrefs.GetInt("Lv" + 3);
        int totalStars2 = PlayerPrefs.GetInt("Lv" + 4) + PlayerPrefs.GetInt("Lv" + 5) + PlayerPrefs.GetInt("Lv" + 6);
        int totalStars3 = PlayerPrefs.GetInt("Lv" + 7) + PlayerPrefs.GetInt("Lv" + 6) + PlayerPrefs.GetInt("Lv" + 9);
        unlockedStarsText[0].text = totalStars1.ToString() + "/ 9";
        unlockedStarsText[1].text = totalStars2.ToString() + "/ 9";
        unlockedStarsText[2].text = totalStars3.ToString() + "/ 9";
    }
    private void UpdateStars()
    {
        stars = PlayerPrefs.GetInt("Lv" + 1) + PlayerPrefs.GetInt("Lv" + 2) + PlayerPrefs.GetInt("Lv" + 3) + PlayerPrefs.GetInt("Lv" + 4) + PlayerPrefs.GetInt("Lv" + 5) + PlayerPrefs.GetInt("Lv" + 6) + PlayerPrefs.GetInt("Lv" + 7) + PlayerPrefs.GetInt("Lv" + 8) + PlayerPrefs.GetInt("Lv" + 9) + PlayerPrefs.GetInt("Lv" + 10) + PlayerPrefs.GetInt("Lv" + 11) + PlayerPrefs.GetInt("Lv" + 12) + PlayerPrefs.GetInt("Lv" + 13) + PlayerPrefs.GetInt("Lv" + 14) + PlayerPrefs.GetInt("Lv" + 15) + PlayerPrefs.GetInt("Lv" + 16);
        startText.text = stars.ToString();
    }
    public void PressMapButton(int _mapIndex)
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        levelSelectionPanels[_mapIndex].gameObject.SetActive(true);
        mapSelectionPanel.gameObject.SetActive(false);
    }
    public void PressSectionBackButton()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
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
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        mapSelectionPanel.gameObject.SetActive(true);
        modeSelectionPanel.gameObject.SetActive(false);
    }
    public void PressQuestBackButton()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        mapSelectionPanel.gameObject.SetActive(false);
        modeSelectionPanel.gameObject.SetActive(true);
    }

    public void PressCustomModeButton()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        PVPSelectionPanel.gameObject.SetActive(false);
        modeSelectionPanel.gameObject.SetActive(false);
        CustomSelection.gameObject.SetActive(true);
    }

    public void PressPVPModeButton()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        PVPSelectionPanel.gameObject.SetActive(true);
        modeSelectionPanel.gameObject.SetActive(false);
    }
    public void PressPVPBackButton()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        PVPSelectionPanel.gameObject.SetActive(false);
        modeSelectionPanel.gameObject.SetActive(true);
    }
    public void BackLevelSelection()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
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

    public void pressBackButton()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX); 
        SceneManager.LoadScene("LoginScene");
    }

    public void PressCustomBackButton()
    {
        AudioManager.Instance.PlaySFX(cfmClickSFX);
        modeSelectionPanel.gameObject.SetActive(true);
        CustomSelection.gameObject.SetActive(false);
    }

}
