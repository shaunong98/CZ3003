using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public FirebaseManager firebaseManager;
    public bool GameisPaused = false;

    public GameObject pauseMenuUI;
    // Update is called once per frame
    
    void Update()
    {
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Menu()
    {
        pauseMenuUI.SetActive(false);
        GameisPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map Selection");
    }

    public void Save() {
        firebaseManager.UpdateStarsToDb();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }
}
