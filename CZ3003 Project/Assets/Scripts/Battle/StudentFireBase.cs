using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class StudentFireBase : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    //public FirebaseUser User;
    public DatabaseReference DBreference;

    public static StudentFireBase Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
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
        //User = FirebaseManager.User;
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void updateBattlePoints(int points) {
        StartCoroutine(updateUserBattlePoints(points));
    }

    public IEnumerator updateUserBattlePoints(int points)
    {
        //need integrate with jh one!
        FirebaseUser User;
        User = FirebaseManager.User;
        Debug.Log("update user battle");
        int worldNumber = QuestionManager.worldNumber;
        int sectionNumber = QuestionManager.sectionNumber;
        Debug.Log($"{worldNumber}");
        Debug.Log($"{sectionNumber}");
        Debug.Log(User.UserId);
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("BattleStats").Child($"{worldNumber}").Child($"{sectionNumber}").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        Debug.Log("reached here at users");
        if (DBTask.Exception != null)
        {
            Debug.Log("hello");
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("what");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            string initialPoint = snapshot.Child("Points").Value.ToString();
            Debug.Log("initial points");
            Debug.Log(initialPoint);
            int initialPoints = int.Parse(initialPoint);
            if (points > initialPoints) {
                var DBTasks = DBreference.Child("users").Child(User.UserId).Child("BattleStats").Child($"{worldNumber}").Child($"{sectionNumber}").Child("Points").SetValueAsync($"{points}");
                yield return new WaitUntil(predicate: () => DBTasks.IsCompleted);

                if (DBTasks.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTasks.Exception}");
                }
                else
                {
                    //Xp is now updated
                }
            }
        }
        BattleSystem.Instance.onBattleOver += PVPController.Instance.EndBattle;

      
    
       
    }  
}

