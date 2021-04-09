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

public class QuestionManager : MonoBehaviour {

    [SerializeField] int lettersPerSecond;
    [SerializeField] Text questionText;
    //[SerializeField] GameObject answerSelector;
    [SerializeField] List<Text> answerText;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;
    
    public static int correctAnswer;
    public string question;
    public static int worldNumber;
    public static int sectionNumber;
    public static string roomID;

    public int CorrectAnswer {
        get { return correctAnswer; }
    }

    public string Question {
        get { return question; }
    }

    public int WorldNumber {
        get { return worldNumber; }
        set { worldNumber = value; }
    }

    public int SectionNumber {
        get { return sectionNumber; }
        set { sectionNumber = value; }
    }

    public static QuestionManager Instance { get; private set; }

    public void Awake() {
        Instance = this;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
        //User = FirebaseManager.User;
        //DontDestroyOnLoad(this);
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public IEnumerator TypeQuestion(string question) {
       questionText.text = "";
       foreach (var letter in question.ToCharArray()) {
           questionText.text += letter;
           yield return new WaitForSeconds(1f/lettersPerSecond);
       }
   }

    public IEnumerator getQuestionsBaseOnLevel(string difficulty) {
        //Debug.Log("Handlemoveselection");
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
        // DataSnapshot snapshot = DBTask.Result;
        // question = snapshot.Child("Question").Value.ToString();
        // Debug.Log(question);
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
            Debug.Log("reached else");
            questionNum = Random.Range(1, length+1); 
            DataSnapshot snapshots = DBTask.Result;
            question = snapshots.Child($"{questionNum}").Child("Question").Value.ToString();
            Debug.Log(question);
            yield return TypeQuestion(question);
            
            // if (Input.GetKeyDown(KeyCode.Space)) {
            //     Debug.Log("reached here at space");
            //     EnableAnswerSelector(true);
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

    public IEnumerator getQuestionsforCustom(int questionNum) {
        //Debug.Log("Handlemoveselection");
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
            Debug.Log("reached else");
            DataSnapshot snapshot = DBTask.Result;
            question = snapshot.Child($"{questionNum}").Child("Question").Value.ToString();
            Debug.Log("get question");
            Debug.Log(question);
            yield return TypeQuestion(question);
            
            // if (Input.GetKeyDown(KeyCode.Space)) {
            //     Debug.Log("reached here at space");
            //     EnableAnswerSelector(true);
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