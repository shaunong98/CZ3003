using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour{

    [SerializeField] Text timerSeconds;
    private float timer = 300f;
    private bool timerPaused = false;
    //private Text timerSeconds;

    // Use this for initialization
    void Awake()
    {
        timerSeconds = GetComponent<Text>();
    }


    // Update is called once per frame
    void TimerUpdate()
    {
        if (!timerPaused)
        {
            timer -= Time.deltaTime;
        }
        timerSeconds.text = timer.ToString();
        if (timer <= 0)
        {
            SceneManager.LoadScene("Map Selection");
            //Application.LoadLevel(levelToLoad);
        }
    }

    public void EnableTimerText(bool enabled) {
       timerSeconds.enabled = enabled;
    }

    public void wrongAnswer()
    {
        timer -= 10f;
    }

    public void completedLevel()
    {
        timerPaused = true;
        float points = timer;
    }
}
