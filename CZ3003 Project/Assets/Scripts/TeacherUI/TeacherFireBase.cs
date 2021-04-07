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
    public GameObject statisticElement;
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

    string StudentSearched;
    public InputField SearchBar;
    public GameObject OverviewPanel;
    public GameObject StudentPanel;

    public Text OODP_S1_points;
    public Text OODP_S2_points;
    public Text OODP_S3_points;

    public Text SE_S1_points;
    public Text SE_S2_points;
    public Text SE_S3_points;

    public Text SSAD_S1_points;
    public Text SSAD_S2_points;
    public Text SSAD_S3_points;

    public Text OODP_S1_stars;
    public Text OODP_S2_stars;
    public Text OODP_S3_stars;

    public Text SE_S1_stars;
    public Text SE_S2_stars;
    public Text SE_S3_stars;

    public Text SSAD_S1_stars;
    public Text SSAD_S2_stars;
    public Text SSAD_S3_stars;

    public Text Name;

    public Dropdown Worldselection;
    public Dropdown SectionSelection;
    string world;
    string section;

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

            string message = "Missing Question!";
            if (string.IsNullOrWhiteSpace(QuestionInputField.text))
            {
                Warning_Text.text = message;
                SubmitButton.interactable = false;
            }

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

    public void SearchStudent()
    {
        StudentSearched = SearchBar.text;
        Debug.Log(StudentSearched);
        SearchBar.text = "";
        StartCoroutine(ShowInformation(StudentSearched));
    }

    private IEnumerator ShowInformation(string _StudentID)
    {

        //Get Student
        var DBTask = DBreference.Child("users").GetValueAsync();
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
            OODP_S1_points.text = "0";
            OODP_S2_points.text = "0";
            OODP_S3_points.text = "0";
            SE_S1_points.text = "0";
            SE_S2_points.text = "0";
            SE_S3_points.text = "0";
            SSAD_S1_points.text = "0";
            SSAD_S2_points.text = "0";
            SSAD_S3_points.text = "0";

            OODP_S1_stars.text = "0";
            OODP_S2_stars.text = "0";
            OODP_S3_stars.text = "0";
            SE_S1_stars.text = "0";
            SE_S2_stars.text = "0";
            SE_S3_stars.text = "0";
            SSAD_S1_stars.text = "0";
            SSAD_S2_stars.text = "0";
            SSAD_S3_stars.text = "0";
        }
        else
        {
            Debug.Log("hello");
            DataSnapshot snapshot = DBTask.Result;
            /*foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username1 = childSnapshot.Child("username").Value.ToString();
                Debug.Log(username1);
            }*/
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                
                string username = childSnapshot.Child("username").Value.ToString();
                Debug.Log(username);
                int oodpS1pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{1}").Child($"{1}").Child("Points").Value.ToString());
                int oodpS2pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{1}").Child($"{2}").Child("Points").Value.ToString());
                int oodpS3pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{1}").Child($"{3}").Child("Points").Value.ToString());

                int seS1pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{2}").Child($"{1}").Child("Points").Value.ToString());
                int seS2pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{2}").Child($"{2}").Child("Points").Value.ToString());
                int seS3pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{2}").Child($"{3}").Child("Points").Value.ToString());

                int ssadS1pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{3}").Child($"{1}").Child("Points").Value.ToString());
                int ssadS2pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{3}").Child($"{2}").Child("Points").Value.ToString());
                int ssadS3pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{3}").Child($"{3}").Child("Points").Value.ToString());

                int oodpS1starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{1}").Value.ToString());
                int oodpS1starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{2}").Value.ToString());
                int oodpS1starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{3}").Value.ToString());
                int oodpS1starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{4}").Value.ToString());
                int oodpS1stars = oodpS1starsT1 + oodpS1starsT2 + oodpS1starsT3 + oodpS1starsT4;

                int oodpS2starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{5}").Value.ToString());
                int oodpS2starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{6}").Value.ToString());
                int oodpS2starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{7}").Value.ToString());
                int oodpS2starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{8}").Value.ToString());
                int oodpS2stars = oodpS2starsT1 + oodpS2starsT2 + oodpS2starsT3 + oodpS2starsT4;

                int oodpS3starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{9}").Value.ToString());
                int oodpS3starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{10}").Value.ToString());
                int oodpS3starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{11}").Value.ToString());
                int oodpS3starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{12}").Value.ToString());
                int oodpS3stars = oodpS3starsT1 + oodpS3starsT2 + oodpS3starsT3 + oodpS3starsT4;

                int seS1starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{13}").Value.ToString());
                int seS1starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{14}").Value.ToString());
                int seS1starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{15}").Value.ToString());
                int seS1starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{16}").Value.ToString());
                int seS1stars = seS1starsT1 + seS1starsT2 + seS1starsT3 + seS1starsT4;

                int seS2starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{17}").Value.ToString());
                int seS2starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{18}").Value.ToString());
                int seS2starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{19}").Value.ToString());
                int seS2starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{20}").Value.ToString());
                int seS2stars = seS2starsT1 + seS2starsT2 + seS2starsT3 + seS2starsT4;

                int seS3starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{21}").Value.ToString());
                int seS3starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{22}").Value.ToString());
                int seS3starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{23}").Value.ToString());
                int seS3starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{24}").Value.ToString());
                int seS3stars = seS3starsT1 + seS3starsT2 + seS3starsT3 + seS3starsT4;

                int ssadS1starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{25}").Value.ToString());
                int ssadS1starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{26}").Value.ToString());
                int ssadS1starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{27}").Value.ToString());
                int ssadS1starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{28}").Value.ToString());
                int ssadS1stars = ssadS1starsT1 + ssadS1starsT2 + ssadS1starsT3 + ssadS1starsT4;

                int ssadS2starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{29}").Value.ToString());
                int ssadS2starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{30}").Value.ToString());
                int ssadS2starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{31}").Value.ToString());
                int ssadS2starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{32}").Value.ToString());
                int ssadS2stars = ssadS2starsT1 + ssadS2starsT2 + ssadS2starsT3 + ssadS2starsT4;

                int ssadS3starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{33}").Value.ToString());
                int ssadS3starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{34}").Value.ToString());
                int ssadS3starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{35}").Value.ToString());
                int ssadS3starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{36}").Value.ToString());
                int ssadS3stars = ssadS3starsT1 + ssadS3starsT2 + ssadS3starsT3 + ssadS3starsT4;

                if (username == _StudentID)
                {
                    Debug.Log("match");
                    OODP_S1_points.text = oodpS1pts.ToString(); // need change wonky
                    OODP_S2_points.text = oodpS2pts.ToString();
                    OODP_S3_points.text = oodpS3pts.ToString();

                    SE_S1_points.text = seS1pts.ToString();
                    SE_S2_points.text = seS2pts.ToString();
                    SE_S3_points.text = seS3pts.ToString();

                    SSAD_S1_points.text = ssadS1pts.ToString();
                    SSAD_S2_points.text = ssadS2pts.ToString();
                    SSAD_S3_points.text = ssadS3pts.ToString();

                    OODP_S1_stars.text = oodpS1stars.ToString();
                    OODP_S2_stars.text = oodpS2stars.ToString();
                    OODP_S3_stars.text = oodpS3stars.ToString();

                    SE_S1_stars.text = seS1stars.ToString();
                    SE_S2_stars.text = seS2stars.ToString();
                    SE_S3_stars.text = seS3stars.ToString();

                    SSAD_S1_stars.text = ssadS1stars.ToString();
                    SSAD_S2_stars.text = ssadS2stars.ToString();
                    SSAD_S3_stars.text = ssadS3stars.ToString();

                    Name.text = username;
                    break;
                }
                OODP_S1_points.text = "0"; //invalid user
                OODP_S2_points.text = "0";
                OODP_S3_points.text = "0";
                SE_S1_points.text = "0";
                SE_S2_points.text = "0";
                SE_S3_points.text = "0";
                SSAD_S1_points.text = "0";
                SSAD_S2_points.text = "0";
                SSAD_S3_points.text = "0";

                OODP_S1_stars.text = "0";
                OODP_S2_stars.text = "0";
                OODP_S3_stars.text = "0";
                SE_S1_stars.text = "0";
                SE_S2_stars.text = "0";
                SE_S3_stars.text = "0";
                SSAD_S1_stars.text = "0";
                SSAD_S2_stars.text = "0";
                SSAD_S3_stars.text = "0";
                Name.text = "Invalid User";
            }
        }
    }
    public void filter()
    {
        world = Worldselection.options[Worldselection.value].text;
        section = SectionSelection.options[SectionSelection.value].text;
        Debug.Log(world);
        Debug.Log(section);
        StartCoroutine(LoadOverallStatistics(world, section));
    }
    private IEnumerator LoadOverallStatistics(string world, string section)
    {
        var DBTask = DBreference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.Log("Yea");
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                int stars = 0;
                int points = 0;
                string username = childSnapshot.Child("username").Value.ToString();
                Debug.Log(username);
                int oodpS1pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{1}").Child($"{1}").Child("Points").Value.ToString());
                int oodpS2pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{1}").Child($"{2}").Child("Points").Value.ToString());
                int oodpS3pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{1}").Child($"{3}").Child("Points").Value.ToString());

                int seS1pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{2}").Child($"{1}").Child("Points").Value.ToString());
                int seS2pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{2}").Child($"{2}").Child("Points").Value.ToString());
                int seS3pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{2}").Child($"{3}").Child("Points").Value.ToString());

                int ssadS1pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{3}").Child($"{1}").Child("Points").Value.ToString());
                int ssadS2pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{3}").Child($"{2}").Child("Points").Value.ToString());
                int ssadS3pts = int.Parse(childSnapshot.Child("BattleStats").Child($"{3}").Child($"{3}").Child("Points").Value.ToString());

                int oodpS1starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{1}").Value.ToString());
                int oodpS1starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{2}").Value.ToString());
                int oodpS1starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{3}").Value.ToString());
                int oodpS1starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{1}").Child($"{4}").Value.ToString());
                int oodpS1stars = oodpS1starsT1 + oodpS1starsT2 + oodpS1starsT3 + oodpS1starsT4;

                int oodpS2starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{5}").Value.ToString());
                int oodpS2starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{6}").Value.ToString());
                int oodpS2starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{7}").Value.ToString());
                int oodpS2starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{2}").Child($"{8}").Value.ToString());
                int oodpS2stars = oodpS2starsT1 + oodpS2starsT2 + oodpS2starsT3 + oodpS2starsT4;

                int oodpS3starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{9}").Value.ToString());
                int oodpS3starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{10}").Value.ToString());
                int oodpS3starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{11}").Value.ToString());
                int oodpS3starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{1}").Child($"{3}").Child($"{12}").Value.ToString());
                int oodpS3stars = oodpS3starsT1 + oodpS3starsT2 + oodpS3starsT3 + oodpS3starsT4;

                int seS1starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{13}").Value.ToString());
                int seS1starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{14}").Value.ToString());
                int seS1starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{15}").Value.ToString());
                int seS1starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{1}").Child($"{16}").Value.ToString());
                int seS1stars = seS1starsT1 + seS1starsT2 + seS1starsT3 + seS1starsT4;

                int seS2starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{17}").Value.ToString());
                int seS2starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{18}").Value.ToString());
                int seS2starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{19}").Value.ToString());
                int seS2starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{2}").Child($"{20}").Value.ToString());
                int seS2stars = seS2starsT1 + seS2starsT2 + seS2starsT3 + seS2starsT4;

                int seS3starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{21}").Value.ToString());
                int seS3starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{22}").Value.ToString());
                int seS3starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{23}").Value.ToString());
                int seS3starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{2}").Child($"{3}").Child($"{24}").Value.ToString());
                int seS3stars = seS3starsT1 + seS3starsT2 + seS3starsT3 + seS3starsT4;

                int ssadS1starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{25}").Value.ToString());
                int ssadS1starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{26}").Value.ToString());
                int ssadS1starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{27}").Value.ToString());
                int ssadS1starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{1}").Child($"{28}").Value.ToString());
                int ssadS1stars = ssadS1starsT1 + ssadS1starsT2 + ssadS1starsT3 + ssadS1starsT4;

                int ssadS2starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{29}").Value.ToString());
                int ssadS2starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{30}").Value.ToString());
                int ssadS2starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{31}").Value.ToString());
                int ssadS2starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{2}").Child($"{32}").Value.ToString());
                int ssadS2stars = ssadS2starsT1 + ssadS2starsT2 + ssadS2starsT3 + ssadS2starsT4;

                int ssadS3starsT1 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{33}").Value.ToString());
                int ssadS3starsT2 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{34}").Value.ToString());
                int ssadS3starsT3 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{35}").Value.ToString());
                int ssadS3starsT4 = int.Parse(childSnapshot.Child("stars").Child($"{3}").Child($"{3}").Child($"{36}").Value.ToString());
                int ssadS3stars = ssadS3starsT1 + ssadS3starsT2 + ssadS3starsT3 + ssadS3starsT4;
                if (world == "OODP")
                {
                    switch (section)
                    {
                        case "1":
                            {
                                points = oodpS1pts;
                                stars = oodpS1stars;
                                break;
                            }
                        case "2":
                            {
                                points = oodpS2pts;
                                stars = oodpS2stars;
                                break;
                            }
                        case "3":
                            {
                                points = oodpS3pts;
                                stars = oodpS3stars;
                                break;
                            }
                        case "TOTAL":
                            {
                                points = oodpS1pts + oodpS2pts + oodpS3pts;
                                stars = oodpS1stars + oodpS2stars + oodpS3stars;
                                break;
                            }
                    }

                }
                else if (world == "SE")
                {
                    switch (section)
                    {
                        case "1":
                            {
                                points = seS1pts;
                                stars = seS1stars;
                                break;
                            }
                        case "2":
                            {
                                points = seS2pts;
                                stars = seS2stars;
                                break;
                            }
                        case "3":
                            {
                                points = seS3pts;
                                stars = seS3stars;
                                break;
                            }
                        case "TOTAL":
                            {
                                points = seS1pts + seS2pts + seS3pts;
                                stars = seS1stars + seS2stars + seS3stars;
                                break;
                            }
                    }
                }
                else if (world == "SSAD")
                {
                    switch (section)
                    {
                        case "1":
                            {
                                points = ssadS1pts;
                                stars = ssadS1stars;
                                break;
                            }
                        case "2":
                            {
                                points = ssadS2pts;
                                stars = ssadS2stars;
                                break;
                            }
                        case "3":
                            {
                                points = ssadS3pts;
                                stars = ssadS3stars;
                                break;
                            }
                        case "TOTAL":
                            {
                                points = ssadS1pts + ssadS2pts + ssadS3pts;
                                stars = ssadS1stars + ssadS2stars + ssadS3stars;
                                break;
                            }
                    }
                }
                else if (world == "Total")
                {
                    switch (section)
                    {
                        case "1":
                            {
                                points = ssadS1pts + oodpS1pts + seS1pts;
                                stars = ssadS1stars + oodpS1stars + seS1stars;
                                break;
                            }
                        case "2":
                            {
                                points = ssadS2pts + oodpS2pts + seS2pts;
                                stars = ssadS2stars + oodpS2stars + seS2stars;
                                break;
                            }
                        case "3":
                            {
                                points = ssadS3pts + oodpS3pts + seS3pts;
                                stars = ssadS3stars + oodpS3stars + seS3stars;
                                break;
                            }
                        case "TOTAL":
                            {
                                points = ssadS1pts + oodpS1pts + seS1pts + ssadS2pts + oodpS2pts + seS2pts + ssadS3pts + oodpS3pts + seS3pts;
                                stars = ssadS1stars + oodpS1stars + seS1stars + ssadS2stars + oodpS2stars + seS2stars + ssadS3stars + oodpS3stars + seS3stars;
                                break;
                            }
                    }
                }
                Debug.Log(stars);
                Debug.Log(world);
                GameObject scoreboardElement = Instantiate(statisticElement, scoreboardContent);
                scoreboardElement.GetComponent<statisticElement>().NewScoreElement(username, stars, points);
            }
        }
    }
}

///// FIX ERROR HANDLING!!!!!