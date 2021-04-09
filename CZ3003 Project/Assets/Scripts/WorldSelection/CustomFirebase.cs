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

        if (enterRoomID != null) 
        {
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
    }
   
}