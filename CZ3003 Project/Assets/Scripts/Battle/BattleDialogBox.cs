using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

public class BattleDialogBox : MonoBehaviour
{
   [SerializeField] Text timerSeconds;
   public float timer = 20f;
   public bool timerPaused = false;
   public int points = 0;

   public bool TimerPaused {
       get {return timerPaused;}
   }

   public float Timer {
        get { return timer; }
        set { timer = value; }
    }

    public int Points {
        get { return points; }
        set { points = value; }
    }

   [SerializeField] int lettersPerSecond;
   [SerializeField] Color highlightedColor; 

   [SerializeField] Text dialogText;
   [SerializeField] Text questionText;
   [SerializeField] GameObject moveSelector;
   [SerializeField] GameObject answerSelector; 
   [SerializeField] GameObject actionSelector; 

   [SerializeField] List<Text> actionText;
   [SerializeField] List<Text> moveText;
   [SerializeField] List<Text> answerText;

    public List<Text> AnswerText { get; set; }

    public void SetTimer(string time) {
        timerSeconds.text = time;
    }

    public IEnumerator TypeQuestion(string question) {
       questionText.text = "";
       foreach (var letter in question.ToCharArray()) {
           questionText.text += letter;
           yield return new WaitForSeconds(1f/lettersPerSecond);
       }
   }

   public IEnumerator TypeDialog(string dialog) {
       dialogText.text = "";
       foreach (var letter in dialog.ToCharArray()) {
           dialogText.text += letter;
           yield return new WaitForSeconds(1f/lettersPerSecond);
       }
   }

    public void SetMoveNames() {
        moveText[0].text =  "Easy";
        moveText[1].text =  "Medium";
        moveText[2].text =  "Hard";
   }

    public int SetAnswer(QuestionBase question) {
        int answer = 0;
        List<int> list = new List<int>();   //  Declare list
 
        for (int n = 0; n < 3; n++)    //  Populate list
        {
            list.Add(n);
        }
        int count = 0;
        for (int ind = 0; ind < 3; ++ind) {
            int index = Random.Range(0, list.Count);    //  Pick random element from the list
            int i = list[index]; //  i = the number that was randomly picked
            list.RemoveAt(index); 
            if (count == 0) {
                answerText[i].text = $"{i}. {question.Answer}";
                answer = i;
                ++count;
            }
            else if (count == 1) {
                answerText[i].text = $"{i}. {question.WrongAnswer1}";
                ++count;
            } 
            else if (count == 2) {
                answerText[i].text = $"{i}. {question.WrongAnswer2}";
                ++count;
            } 
        }
        return answer;   
   }

//    public bool isAnswer(QuestionBase question, string answer) {
//        if (answer == question.Answer) {
//            return true;
//        }
//        return false;
//    }

    public void EnableTimerText(bool enabled) {
       timerSeconds.enabled = enabled;
   }

   public void EnableDialogText(bool enabled) {
       dialogText.enabled = enabled;
   }

    public void EnableQuestionText(bool enabled) {
        Debug.Log("enabled text");
       questionText.enabled = enabled;
   }

   public void EnableAnswerSelector(bool enabled) {
       answerSelector.SetActive(enabled);
   }

   public void EnableActionSelector(bool enabled) {
       actionSelector.SetActive(enabled);
   }

   public void EnableMoveSelector(bool enabled) {
       moveSelector.SetActive(enabled);
   }

   public void UpdateActionSelection(int selectedAction) {
       for (int i = 0; i < actionText.Count; ++i) {
           if (i == selectedAction) {
               actionText[i].color = highlightedColor;
           }
           else
                actionText[i].color = Color.black;
       }
   }

   public void UpdateMoveSelection(int selectedMove) {
       for (int i = 0; i < moveText.Count; ++i) {
           if (i == selectedMove) {
               moveText[i].color = highlightedColor;
           }
           else
                moveText[i].color = Color.black;
       }
   }

   public void UpdateAnswerSelection(int selectedAnswer) {
       for (int i = 0; i < answerText.Count; ++i) {
           if (i == selectedAnswer) {
               answerText[i].color = highlightedColor;
           }
           else
                answerText[i].color = Color.black;
       }
   }

   public void completedLevel()
    {
        timerPaused = true;
        points = (int)timer;
    } 
}
