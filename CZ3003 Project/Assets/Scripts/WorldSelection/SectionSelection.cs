using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class SectionSelection : MonoBehaviour
{
    public LevelLoader levelLoader;
    [Header("Sounds")]
    [SerializeField] private AudioClip cfmClickSFX;
    [SerializeField] private AudioClip ErrorClickSFX;
    public int worldNumber;
    public int sectionNumber;
    public bool isUnlocked = false;
    public Image lockImage;
    public Image[] starsImage;
    public Sprite[] starsSprite;

    public void Update()
    {
        UpdateLevelButton();
        UnlockLevel();
    }

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

    private void UnlockLevel()
    {
        int previousLvIndex = int.Parse(gameObject.name) - 1; // get previous level
        if(PlayerPrefs.GetInt("Lv"+previousLvIndex)>3)
        {
            isUnlocked = true;
        }
    }
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
