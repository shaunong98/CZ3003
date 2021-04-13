// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class BattleDialogBox : MonoBehaviour
{
    // Text timerSeconds is displayed on the battle screen to show how much time is left for the player.
   [SerializeField] Text timerSeconds;

    // To control speed at which letters are printing out.
   [SerializeField] int lettersPerSecond;

   // Highlighted color when you are pointing on that text.
   [SerializeField] Color highlightedColor; 

   [SerializeField] Text dialogText;
   [SerializeField] Text questionText;

   // Move selector contains Easy Medium Hard.
   [SerializeField] GameObject moveSelector;

   // Run selector contains the answers to the question.
   [SerializeField] GameObject answerSelector; 

   // Action Selector contains Fight and Run text.
   [SerializeField] GameObject actionSelector; 

   [SerializeField] List<Text> actionText;
   [SerializeField] List<Text> moveText;
   [SerializeField] List<Text> answerText;

    public List<Text> AnswerText { get; set; }

    public float timer = 20f;

    // bool variable timer being paused.
    public bool timerPaused = false;

    // Points obtained by student depending on time left during PVP battlescene.
    public int points = 0;

    // Able to access this boolean variable from another class.
    public bool TimerPaused {
        get {return timerPaused;}
    }

    // Able to access timer from another class.
    public float Timer {
        get { return timer; }
        set { timer = value; }
    }

    // Able to access points from another class.
    public int Points {
        get { return points; }
        set { points = value; }
    }

    // Setting timer text on unity UI to the time.
    public void SetTimer(string time) {
        timerSeconds.text = time;
    }

    // Method to print out the words one by one in the dialog box.
    public IEnumerator TypeDialog(string dialog) {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray()) {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }

    // Setting move names to be Easy Medium and Hard
    public void SetMoveNames() {
        moveText[0].text =  "Easy";
        moveText[1].text =  "Medium";
        moveText[2].text =  "Hard";
    }

    // Method to enable timer text.
    public void EnableTimerText(bool enabled) {
       timerSeconds.enabled = enabled;
    }

    // Method to enable dialog text.
    public void EnableDialogText(bool enabled) {
       dialogText.enabled = enabled;
    }

    // Method to enable question text.
    public void EnableQuestionText(bool enabled) {
        Debug.Log("enabled text");
        questionText.enabled = enabled;
    }

    // Method to enable answer gameoject
    public void EnableAnswerSelector(bool enabled) {
       answerSelector.SetActive(enabled);
    }

    // Method to enable action gameobject.
    public void EnableActionSelector(bool enabled) {
       actionSelector.SetActive(enabled);
    }

    // Method to enable move gameobject.
    public void EnableMoveSelector(bool enabled) {
       moveSelector.SetActive(enabled);
    }

    // Method to change action text to blue as the player presses W or S.
    public void UpdateActionSelection(int selectedAction) {
       for (int i = 0; i < actionText.Count; ++i) {
           if (i == selectedAction) {
               actionText[i].color = highlightedColor;
           }
           else
                actionText[i].color = Color.black;
       }
   }

    // Method to change move text to blue as the player presses A or D.
    public void UpdateMoveSelection(int selectedMove) {
       for (int i = 0; i < moveText.Count; ++i) {
           if (i == selectedMove) {
               moveText[i].color = highlightedColor;
           }
           else
                moveText[i].color = Color.black;
       }
    }

    // Method to change answer text to blue as the player presses A or D.
    public void UpdateAnswerSelection(int selectedAnswer) {
       for (int i = 0; i < answerText.Count; ++i) {
           if (i == selectedAnswer) {
               answerText[i].color = highlightedColor;
           }
           else
                answerText[i].color = Color.black;
       }
    }

    // Reset Answer text to empty whenever a new question is being displayed.
    public void RestartAnswerSelection() {
       for (int i = 0; i < answerText.Count; ++i) {
            answerText[i].text = "";
       }
    }
   
    // Method to convert the time left to points for the player.
    public void completedLevel()
    {
        timerPaused = true;
        points = (int)timer;
    } 
}
