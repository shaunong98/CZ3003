using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
//Done by Jun Hao and Jethro

public class SectionSelection : MonoBehaviour
{
    //This is an instance of level loader
    public LevelLoader levelLoader;
    [Header("Sounds")]
    //This is the gameplay music
    [SerializeField] private AudioClip cfmClickSFX;
    //This is the gameplay music
    [SerializeField] private AudioClip ErrorClickSFX;
    //This variable stores the world that is selected
    public int worldNumber;
    //This variable stores the section that is selected
    public int sectionNumber;
    //This variable determines if that section is unlocked
    public bool isUnlocked = false;
    //This variable is the locked image
    public Image lockImage;
    //This image is the hollow stars image
    public Image[] starsImage;
    //This images is the filled star image
    public Sprite[] starsSprite;
    //This method is called to update the level
    public void Update()
    {
        UpdateLevelButton();
        UnlockLevel();
    }
    //This method checks if the section is unlocked and how many stars are gained
    private void UpdateLevelButton()
    {
        if(isUnlocked)
        {
            int totalstar = PlayerPrefs.GetInt("Lv"+gameObject.name);
            Debug.Log($"{totalstar}");
            Debug.Log("Lv"+gameObject.name);
            lockImage.gameObject.SetActive(false);
            for (int i=0; i< starsImage.Length; i++)
            {
                starsImage[i].gameObject.SetActive(true);
            }
            for (int i=0;i<PlayerPrefs.GetInt("Lv"+gameObject.name);i++)
            {
                starsImage[i].sprite = starsSprite[i];
            }
        }else
        {
            lockImage.gameObject.SetActive(true);
            for (int i = 0; i < starsImage.Length; i++)
            {
                starsImage[i].gameObject.SetActive(false);
            }
        }
    }
    //This method determines if the stage can be unlocked
    private void UnlockLevel()
    {
        int previousLvIndex = int.Parse(gameObject.name) - 1; // get previous level
        if(PlayerPrefs.GetInt("Lv"+previousLvIndex)>3)
        {
            isUnlocked = true;
        }
    }
    //This method is called when the player starts the gameplay
    public void SceneTransition(string _sceneName)
    {
        if(isUnlocked)
        {
            AudioManager.Instance.PlaySFX(cfmClickSFX);
            WorldUIManager.instance.setAllToFalse();
            QuestionManager.worldNumber = worldNumber;
            QuestionManager.sectionNumber = sectionNumber;
            levelLoader.Loadlevel(_sceneName);
        }
        else
        {
            AudioManager.Instance.PlaySFX(ErrorClickSFX);
            Debug.Log("not unlocked");
        }
    }
}
