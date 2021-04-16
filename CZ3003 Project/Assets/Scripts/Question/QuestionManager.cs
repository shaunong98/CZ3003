// Authors: Jethro
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Question Manager class that interacts with the firebase to extract the questions and answers.
public class QuestionManager : MonoBehaviour 
{
    // Determine the speed at which the letters are being printed out onto the dialog box.
    [SerializeField] int lettersPerSecond;

    // Question text object.
    [SerializeField] Text questionText;

    // Answer text object.
    [SerializeField] List<Text> answerText;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;
    
    // Creating an instance for Question Manager to be called in another class.
    public static QuestionManager Instance { get; private set; }

    // Index of correct answer.
    public static int correctAnswer;

    // Question to be displayed.
    public string question;

    // World number (1, 2 or 3)
    public static int worldNumber;

    // Section number (1, 2 or 3)
    public static int sectionNumber;

    // Room ID of custom battle scene.
    public static string roomID;

    // Getter method for correct answer.
    public int CorrectAnswer {
        get { return correctAnswer; }
    }

    // Getter method for question.
    public string Question {
        get { return question; }
    }

    // Getter method for world number.
    public int WorldNumber {
        get { return worldNumber; }
        set { worldNumber = value; }
    }

    // Getter method for section number.
    public int SectionNumber {
        get { return sectionNumber; }
        set { sectionNumber = value; }
    }

    // Awake method to be called when this class is instantiated.
    public void Awake() {
        Instance = this;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Initializing Firebase.
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Method to print out the words one by one in the question box.
    public IEnumerator TypeQuestion(string question) {
       questionText.text = "";
       foreach (var letter in question.ToCharArray()) {
           questionText.text += letter;
           yield return new WaitForSeconds(1f/lettersPerSecond);
       }
   }

    // Method to get questions & answer base on the level of difficulty chosen by the player.
    // Answers and questions to be randomized.
    public IEnumerator getQuestionsBaseOnLevel(string difficulty) 
    {
        int questionNum;
        Debug.Log("reached here at getquestion");
        Debug.Log($"{worldNumber}");
        Debug.Log($"{sectionNumber}");
        Debug.Log(difficulty);
        Debug.Log($"{question}");
        var DBTask = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        Debug.Log("reached completed");
        DataSnapshot snapshot = DBTask.Result;
        int length = (int)snapshot.ChildrenCount;
        Debug.Log($"{length}");
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.LogWarning(message: $"Failed to register task");
        }
        else
        {
            // Data has been retrieved
            questionNum = Random.Range(1, length+1); 
            DataSnapshot snapshots = DBTask.Result;
            question = snapshots.Child($"{questionNum}").Child("Question").Value.ToString();
            Debug.Log(question);
            yield return TypeQuestion(question);
            
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
                    answerText[i].text = $"{i}. {snapshot.Child($"{questionNum}").Child("A1").Value.ToString()}";
                    BattleSystem.correctAnswer = i;
                    ++count;
                }
                else if (count == 1) {
                    answerText[i].text = $"{i}. {snapshot.Child($"{questionNum}").Child("A2").Value.ToString()}";
                    ++count;
                } 
                else if (count == 2) {
                    answerText[i].text = $"{i}. {snapshot.Child($"{questionNum}").Child("A3").Value.ToString()}";
                    ++count;
                } 
            }
  
        }
        yield return new WaitForSeconds(1f);
        
    }

    // Method to get questions & answer from the firebase.
    // Questions to be given in order; Answers to be randomized.
    public IEnumerator getQuestionsforCustom(int questionNum) 
    {
        Debug.Log(roomID);
        Debug.Log($"{questionNum}");
        Debug.Log("reached here at getquestion");
        var DBTask = DBreference.Child("custom").Child(roomID).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        Debug.Log("reached completed");
        DataSnapshot snapshots = DBTask.Result;
        int str = 0;
        foreach (DataSnapshot childSnapshot in snapshots.Children.Reverse<DataSnapshot>()) {
            if (childSnapshot.Key.ToString() == "users") {
                str++;
            }
            if (childSnapshot.Key.ToString() == "usercreated") {
                str++;
            }
        }
        int length = (int)snapshots.ChildrenCount;
        Debug.Log($"{length}");
        CustomBattleSystem.totalQuestionNum = length - str;
        Debug.Log($"{CustomBattleSystem.totalQuestionNum}");
        
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.LogWarning(message: $"Failed to register task");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            question = snapshot.Child($"{questionNum}").Child("Question").Value.ToString();
            Debug.Log("get question");
            Debug.Log(question);
            yield return TypeQuestion(question);
            
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
                    answerText[i].text = $"{i}. {snapshot.Child($"{questionNum}").Child("A1").Value.ToString()}";
                    CustomBattleSystem.correctAnswer = i;
                    ++count;
                }
                else if (count == 1) {
                    answerText[i].text = $"{i}. {snapshot.Child($"{questionNum}").Child("A2").Value.ToString()}";
                    ++count;
                } 
                else if (count == 2) {
                    answerText[i].text = $"{i}. {snapshot.Child($"{questionNum}").Child("A3").Value.ToString()}";
                    ++count;
                } 
            }
  
        }
        CustomBattleSystem.questionNum = questionNum + 1;
        yield return new WaitForSeconds(1f);
    }

}