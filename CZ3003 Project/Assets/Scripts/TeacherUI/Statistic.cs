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

public class Statistic : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    //User Data variables
    [Header("UserData")]
    public TMP_InputField usernameField;
    public TMP_InputField xpField;
    public TMP_InputField killsField;
    public TMP_InputField masteryField;
    public GameObject scoreElement;
    public Transform scoreboardContent;

    //Local Data
    string StudentSearched;
    public InputField SearchBar;
    public GameObject OverviewPanel;
    public GameObject StudentPanel;
    public GameObject[] Students;
    public Text[] StudentId;
    public Text Statistic1;
    public Text Statistic2;
    public Text Statistic3;

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
    public void SearchStudent()
    {
        StudentSearched = SearchBar.text;
        //Debug.Log(StudentSearched);
        SearchBar.text = "";
        DisplayStudentStatistic();
    }
    public void DisplayStudentStatistic()
    {
        StartCoroutine(ShowInformation(StudentSearched));
        StudentPanel.gameObject.SetActive(true);
        OverviewPanel.gameObject.SetActive(false);
    }
    public void ClickonStudent(int index)
    {
        StudentSearched = StudentId[index].text.ToString();
        Debug.Log(StudentSearched);
        DisplayStudentStatistic();
    }
    private IEnumerator ShowInformation(string _StudentID)
    {
        //Debug.Log(_StudentID);
        //Get Student
        var DBTask = DBreference.Child("users").Child(_StudentID).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        
        Debug.Log("something");


        if (DBTask.Exception != null)
        {
            Debug.Log("Yea");
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("Yo");
            Statistic3.text = "XP = 0";
            Statistic1.text = "Kills = 0";
            Statistic2.text = "Mastery = 0";
        }
        else
        {
            Debug.Log("heelo");
            DataSnapshot snapshot = DBTask.Result;
            Statistic1.text = "Kills = " + snapshot.Child("kills").Value.ToString();
            Statistic2.text = "Mastery = " + snapshot.Child("mastery").Value.ToString();
            Statistic3.text = "XP = " + snapshot.Child("xp").Value.ToString();
        }
    }
    public void ClickBackbutton()
    {
        StudentPanel.gameObject.SetActive(false);
        OverviewPanel.gameObject.SetActive(true);
    }
}

