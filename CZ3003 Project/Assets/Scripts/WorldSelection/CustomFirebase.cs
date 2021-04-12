using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class CustomFirebase : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public static FirebaseUser User;
    public DatabaseReference DBreference;

    //custom variables
    [Header("custom")]
    public TMP_InputField createRoomID;
    public TMP_InputField enterRoomID;
    public TMP_Text warningText;
    public TMP_Text confirmText;
    bool roomfound = false;
    bool userfound = false;

    public static string createdUsername;

    public GameObject questionElement;
    public Transform questionListContent;
    string Question;
    string A1;
    string A2;
    string A3;
    int questionNo;
    bool roomexist;
    public Text errormsg;
    public GameObject StartPanel;
    public GameObject SelectionPanel;
    public string Room;
    
    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
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
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void enterRoomButtonMethod()
    {
        StartCoroutine(checkRoomID());
        // if (enterRoomID != null) {
        //     QuestionManager.roomID = enterRoomID.text;
        //     SceneManager.LoadScene("CustomBattleScene");
        // }
        //SceneManager.LoadScene("CustomBattleScene");
        //StartCoroutine(createQuestionsAndAnswers(QuestionInputField.text, AnswerInputField1.text, AnswerInputField2.text, AnswerInputField3.text));
        
    }

    public IEnumerator checkRoomID() {
        userfound = false;
        FirebaseUser User;
        User = FirebaseManager.User;
        string username = "";
        var aTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => aTask.IsCompleted);
        Debug.Log("reached here at users");
        if (aTask.Exception != null)
        {
            Debug.Log("hello");
            Debug.LogWarning(message: $"Failed to register task with {aTask.Exception}");
        }
        else if (aTask.Result.Value == null)
        {
            Debug.Log("what");
        }
        else
        {
            DataSnapshot snapshot = aTask.Result;
            username = snapshot.Child("username").Value.ToString();
        }

        if (enterRoomID.text != "") 
        {
            Debug.Log("null");
            var DBTask = DBreference.Child("custom").GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            Debug.Log("something");

            if (DBTask.Exception != null)
            {
                Debug.Log("Yea");
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {

            }
            else 
            {
                DataSnapshot snapshot = DBTask.Result;
                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                {
                    string roomid = childSnapshot.Key.ToString();
                    Debug.Log("roomid");
                    Debug.Log(roomid);
                    if (roomid == enterRoomID.text) {
                        roomfound = true;
                        createdUsername = childSnapshot.Child("usercreated").Value.ToString();
                        Debug.Log(createdUsername);
                        var DBTasks = DBreference.Child("custom").Child(roomid).Child("users").GetValueAsync();
                        yield return new WaitUntil(predicate: () => DBTasks.IsCompleted);
                        DataSnapshot snapshots = DBTasks.Result;
                        Debug.Log(username);
                        foreach (DataSnapshot childSnapshots in snapshots.Children.Reverse<DataSnapshot>()) {
                            string users = childSnapshots.Key.ToString();
                            Debug.Log(users);
                            if (username == users) {
                                userfound = true;
                            }
                        }

                    }
                }
                if (roomfound) 
                {
                    if (!userfound){
                        QuestionManager.roomID = enterRoomID.text;
                        warningText.text = "";
                        confirmText.text = "RoomID found. Directing to BattleRoom...";
                        yield return new WaitForSeconds(2f);
                        confirmText.text = "";
                        SceneManager.LoadScene("CustomBattleScene");
                    }
                    else {
                        warningText.text = "You have attempted already!";
                        yield return new WaitForSeconds(1f);
                        warningText.text = "";
                    }
                }
                else {
                    Debug.Log("Not found");
                    warningText.text = "RoomID not found!";
                    yield return new WaitForSeconds(1f);
                    warningText.text = "";
                }
            }
        }
        else {
            warningText.text = "Please enter a roomID!";
            yield return new WaitForSeconds(1f);
            warningText.text = "";
        }
    }
    public void displayallquestions(){
        StartCoroutine(filterquestions());
    }
    public IEnumerator filterquestions(){
        int world = CreateRoom.World;
        int section = CreateRoom.Section;
        string difficulty = CreateRoom.Difficulty;

         var DBTask = DBreference.Child("Qns").Child($"{world}").Child($"{section}").Child(difficulty).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log("Yea");
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (Transform child in questionListContent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string question = childSnapshot.Child("Question").Value.ToString();
                Debug.Log(question);
                GameObject questionListElement = Instantiate(questionElement, questionListContent);
                questionListElement.GetComponent<QuestionPicker>().NewQuestionItem(question);
            }
        }
    }
    public void newRoom()
    {
        questionNo = 0;
    }
    public void Addquestion(string _question){
        Question = _question;
        Debug.Log(Question);
        StartCoroutine(LoadQuestionAndAnswer());
        StartCoroutine(PushtoDB());
    }
    public IEnumerator LoadQuestionAndAnswer(){
        int world = CreateRoom.World;
        int section = CreateRoom.Section;
        string difficulty = CreateRoom.Difficulty;

         var DBTask = DBreference.Child("Qns").Child($"{world}").Child($"{section}").Child(difficulty).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log("Yea");
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
           
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string question = childSnapshot.Child("Question").Value.ToString();
                if (question == Question)
                {
                    A1 = childSnapshot.Child("A1").Value.ToString();
                    A2 = childSnapshot.Child("A2").Value.ToString();
                    A3 = childSnapshot.Child("A3").Value.ToString();
                    Debug.Log(A1);
                }
            }
        }
    }
    public IEnumerator PushtoDB(){
        int world = CreateRoom.World;
        int section = CreateRoom.Section;
        string difficulty = CreateRoom.Difficulty;
        questionNo = questionNo + 1;

        var qnTask = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("Question").SetValueAsync(Question);

        yield return new WaitUntil(predicate: () => qnTask.IsCompleted);

        if (qnTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {qnTask.Exception}");

            /*string message = "Missing Question!";
            if (string.IsNullOrWhiteSpace(QuestionInputField.text))
            {
                Warning_Text.text = message;
                SubmitButton.interactable = false;
            }*/

        }

        var a1Task = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("A1").SetValueAsync(A1);

        yield return new WaitUntil(predicate: () => a1Task.IsCompleted);

        if (a1Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a1Task.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField1.text))
            {
                Warning_Text.text = message;
            }*/
        }

        var a2Task = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("A2").SetValueAsync(A2);

        yield return new WaitUntil(predicate: () => a2Task.IsCompleted);

        if (a2Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a2Task.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField2.text))
            {
                Warning_Text.text = message;
            }*/
        }

        var a3Task = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("A3").SetValueAsync(A3);

        yield return new WaitUntil(predicate: () => a3Task.IsCompleted);

        if (a3Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a3Task.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField3.text))
            {
                Warning_Text.text = message;
            }*/
        }

        FirebaseUser User;
        User = FirebaseManager.User;
        string username = "";
        var aTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => aTask.IsCompleted);
        Debug.Log("reached here at users");
        if (aTask.Exception != null)
        {
            Debug.Log("hello");
            Debug.LogWarning(message: $"Failed to register task with {aTask.Exception}");
        }
        else if (aTask.Result.Value == null)
        {
            Debug.Log("what");
        }
        else
        {
            DataSnapshot snapshot = aTask.Result;
            username = snapshot.Child("username").Value.ToString();
        }
        var UCTask = DBreference.Child("custom").Child(Room).Child("usercreated").SetValueAsync(username);

        yield return new WaitUntil(predicate: () => UCTask.IsCompleted);

        if (UCTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {UCTask.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField3.text))
            {
                Warning_Text.text = message;
            }*/
        }
        var UserTask = DBreference.Child("custom").Child(Room).Child("users").SetValueAsync(null);

        yield return new WaitUntil(predicate: () => UserTask.IsCompleted);

        if (UserTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {UserTask.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField3.text))
            {
                Warning_Text.text = message;
            }*/
        }
    }
    public void checkRoomExist(){
        StartCoroutine(CheckExistingRoom());
    }
    private IEnumerator CheckExistingRoom(){
        Room = createRoomID.text;
        if (Room == "")
        {
            errormsg.text = "Please input something";
        }
        else{
            var DBTask = DBreference.Child("custom").GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
            
            roomexist = false;
            if (DBTask.Exception != null)
            {
                Debug.Log("Yea");
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else{
                DataSnapshot snapshot = DBTask.Result;
                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                {
                    string roomid = childSnapshot.Key.ToString();
                    if (roomid == Room) {
                        roomexist = true;
                        errormsg.text = "Room already exist!";
                    }
                }
                if (roomexist != true)
                {
                SelectionPanel.gameObject.SetActive(true);
                StartPanel.gameObject.SetActive(false);
                errormsg.text = "";
                }
            }
        }
        
    }
   
}