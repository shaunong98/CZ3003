// Authors: Jethro, Jun Hao
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
//Done by Jun Hao and Jethro

public class CustomFirebase : MonoBehaviour
{
    [Header("Firebase")]
    //Variable for DependencyStatus
    public DependencyStatus dependencyStatus;
    //Variable for FirebaseAuth 
    public FirebaseAuth auth;  
    //Variable for user for firebase  
    public static FirebaseUser User;
    //Variable for Database reference
    public DatabaseReference DBreference;

    //custom variables
    [Header("custom")]
    //This is the input field for the create room
    public TMP_InputField createRoomID;
    //This is the input field for the enter room
    public TMP_InputField enterRoomID;
    //This is the warning text
    public TMP_Text warningText;
    //This is the confirmation text
    public TMP_Text confirmText;
    //This is a variable to check if room is found
    bool roomfound = false;
    //This is a variable to check if user is found
    bool userfound = false;
    //This is an instance of level loader
    public LevelLoader battleLoader;
    //This is the username of room creator
    public static string createdUsername;
    //This component is instantiated in the scroll view in the question display panel
    public GameObject questionElement;
    //This is the scroll view in the question display panel
    public Transform questionListContent;
    //This is the question selected
    string Question;
    //This is the answer for the question selected
    string A1;
    //This is the answer for the question selected
    string A2;
    //This is the answer for the question selected
    string A3;
    //This is the number of questions in the room
    int questionNo;
    //This variable determines if the room exist
    bool roomexist;
    //This is the starting panel where the user can create or enter room
    public GameObject StartPanel;
    //This is the selection panel for the create room
    public GameObject SelectionPanel;
    //This is the room
    public string Room;
    //This is game music
    [SerializeField] private AudioClip CustomMusic;
    //This method creates an instance of firebase
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

    //This method initialises the firebase
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    //This method is called when the enter button is clicked
    public void enterRoomButtonMethod()
    {
        StartCoroutine(checkRoomID());
        
    }
    //This method checks if room exist
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
                        AudioManager.Instance.PlayMusicWithFade(CustomMusic,0.1f);
                        battleLoader.LoadCustom();
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
    //This method is called to display all the questions after it is filtered
    public void displayallquestions(){
        StartCoroutine(filterquestions());
    }
    //This method is called
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
    //This method set the question number to 0
    public void newRoom()
    {
        questionNo = 0;
    }
    //This method is called when a question is added to a custom room
    public void Addquestion(string _question){
        Question = _question;
        Debug.Log(Question);
        StartCoroutine(LoadQuestionAndAnswer());
        StartCoroutine(PushtoDB());
    }
    //This method loads all questions and answer for the question selected
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
                }
            }
        }
    }
    //This method push the question selected to the firebase
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

        }

        var a1Task = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("A1").SetValueAsync(A1);

        yield return new WaitUntil(predicate: () => a1Task.IsCompleted);

        if (a1Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a1Task.Exception}");
        }

        var a2Task = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("A2").SetValueAsync(A2);

        yield return new WaitUntil(predicate: () => a2Task.IsCompleted);

        if (a2Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a2Task.Exception}");
        }

        var a3Task = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("A3").SetValueAsync(A3);

        yield return new WaitUntil(predicate: () => a3Task.IsCompleted);

        if (a3Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a3Task.Exception}");
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
        }
        var UserTask = DBreference.Child("custom").Child(Room).Child("users").SetValueAsync(null);

        yield return new WaitUntil(predicate: () => UserTask.IsCompleted);

        if (UserTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {UserTask.Exception}");
        }
    }
    //This method checks if the room exists
    public void checkRoomExist(){
        StartCoroutine(CheckExistingRoom());
    }
    //This method checks the room against the firebase backend
    private IEnumerator CheckExistingRoom(){
        Room = createRoomID.text;
        if (Room == "")
        {
            warningText.text = "Please input something";
            yield return new WaitForSeconds(1f);
            warningText.text = "";
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
                        warningText.text = "Room already exist!";
                        yield return new WaitForSeconds(1f);
                        warningText.text = "";
                    }
                }
                if (roomexist != true)
                {
                SelectionPanel.gameObject.SetActive(true);
                StartPanel.gameObject.SetActive(false);
                warningText.text = "";
                }
            }
        }
        
    }
   
}