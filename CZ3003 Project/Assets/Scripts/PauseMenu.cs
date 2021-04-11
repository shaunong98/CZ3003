using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider AudioSlider;
    public Slider SFXSlider;
    
    [SerializeField] private AudioClip levelMusic;
    public LevelLoader battleLoader;
    public FirebaseManager firebaseManager;
    public bool GameisPaused = false;

    public GameObject pauseMenuUI;
    // Update is called once per frame
    
    void Update()
    {
        AudioManager.Instance.SetMusicVolume(AudioSlider.value);
        AudioManager.Instance.SetSFXVolume(SFXSlider.value);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameisPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        AudioManager.Instance.ResumeMusic();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    public void Pause()
    {
        AudioManager.Instance.PauseMusic();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void Quit()
    {
        battleLoader.FadetoBlack();
        Application.Quit();
    }

    public void Menu()
    {
        AudioManager.Instance.PlayMusic(levelMusic);
        pauseMenuUI.SetActive(false);
        GameisPaused = false;
        Time.timeScale = 1f;
        battleLoader.LoadMain();
    }

    public void Save() {
        firebaseManager.UpdateStarsToDb();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }
}
