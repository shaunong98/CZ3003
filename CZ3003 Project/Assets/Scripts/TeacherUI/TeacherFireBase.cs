using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class TeacherFireBase : MonoBehaviour
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

    //Question and Answers variables
    [Header("QnA")]
    public InputField QuestionInputField;
    public InputField AnswerInputField1;
    public InputField AnswerInputField2;
    public InputField AnswerInputField3;
    public TMP_Text Warning_Text;

    public GameObject AddQuestionPanel;
    public GameObject OptionSelectionPanel;
    public GameObject FunctionSelectionPanel;
    public GameObject InfoBox;
    public Button SubmitButton;

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

    public void ClearQuestionAndAnswersFields()
    {
        QuestionInputField.Select();
        QuestionInputField.text = "";
        AnswerInputField1.Select();
        AnswerInputField1.text = "";
        AnswerInputField2.Select();
        AnswerInputField2.text = "";
        AnswerInputField3.Select();
        AnswerInputField3.text = "";
    }
    // Fix error handling!
    void update()
    {
        string message = "Missing Input!";
        if (string.IsNullOrWhiteSpace(QuestionInputField.text) || string.IsNullOrWhiteSpace(AnswerInputField1.text) || string.IsNullOrWhiteSpace(AnswerInputField2.text) || string.IsNullOrWhiteSpace(AnswerInputField3.text))
        {
            Warning_Text.text = message;
            SubmitButton.interactable = false;
        }
        else
        {
            SubmitButton.interactable = true;
            SubmitButtonMethod();
        }
    }

    //Function for the register button
    public void SubmitButtonMethod()
    {
        Debug.Log("Reached here!");
        StartCoroutine(createQuestionsAndAnswers(QuestionInputField.text, AnswerInputField1.text, AnswerInputField2.text, AnswerInputField3.text));
    }

    private IEnumerator createQuestionsAndAnswers(string _question, string _answer1, string _answer2, string _answer3)
    {
        //need integrate with jh one!
        int worldNumber = QuestionAdder.World;
        int sectionNumber = QuestionAdder.Section;
        string difficulty = QuestionAdder.Difficulty;
        Debug.Log(worldNumber);
        Debug.Log(sectionNumber);
        Debug.Log(difficulty);

        //Set the currently logged in user mastery
        var DBTask = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        DataSnapshot snapshot = DBTask.Result;
        int length = (int)snapshot.ChildrenCount;

        var qnTask = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).Child($"{length + 1}").Child("Question").SetValueAsync(_question);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

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

        var a1Task = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).Child($"{length + 1}").Child("A1").SetValueAsync(_answer1);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (a1Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a1Task.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField1.text))
            {
                Warning_Text.text = message;
            }*/
        }

        var a2Task = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).Child($"{length + 1}").Child("A2").SetValueAsync(_answer2);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (a2Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a2Task.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField2.text))
            {
                Warning_Text.text = message;
            }*/
        }

        var a3Task = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).Child($"{length + 1}").Child("A3").SetValueAsync(_answer3);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (a3Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a3Task.Exception}");

            /*string message = "Missing Answer!";
            if (string.IsNullOrEmpty(AnswerInputField3.text))
            {
                Warning_Text.text = message;
            }*/
        }

        ClearQuestionAndAnswersFields();

        AddQuestionPanel.gameObject.SetActive(false);
        OptionSelectionPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);
    }

}

///// FIX ERROR HANDLING!!!!!