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
    public GameObject statisticElement;
    public Transform scoreboardContent;
    public GameObject leaderElement;
    public Transform leaderboardContent;

    //Question and Answers variables
    [Header("QnA")]
    public InputField QuestionInputField;
    public InputField AnswerInputField1;
    public InputField AnswerInputField2;
    public InputField AnswerInputField3;
    //public TMP_Text Warning_Text;

    public GameObject AddQuestionPanel;
    public GameObject OptionSelectionPanel;
    public GameObject FunctionSelectionPanel;
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
    public GameObject questionElement;
    public Transform questionListContent;
    string Question;

    public InputField EditQuestionInputField;
    public InputField EditAnswerInputField1;
    public InputField EditAnswerInputField2;
    public InputField EditAnswerInputField3;
    public GameObject EditQuestionPanel;
    public GameObject DisplayQuestionPanel;
    int totalquestion = 0;
    int currentindex;

    public GameObject CustomquestionElement;
    public Transform CustomquestionListContent;
    string CustomQuestion;
    string A1;
    string A2;
    string A3;
    int questionNo;
    bool roomexist;
    public Text errormsg;
    public Text errormsg1;
    public GameObject StartPanel;
    public GameObject SelectionPanel;
    string Room;
    public InputField createRoomID;
    public InputField ViewRoomID;
    public GameObject ScorePanel;
    public GameObject studentElement;
    public Transform studentListContent;

    public Text missinginput;

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
        if (QuestionInputField.text != "" & AnswerInputField1.text != "" & AnswerInputField2.text != "" & AnswerInputField3.text != "")
        {
            missinginput.text = "";
            StartCoroutine(createQuestionsAndAnswers(QuestionInputField.text, AnswerInputField1.text, AnswerInputField2.text, AnswerInputField3.text));
        }
        else 
        {
            missinginput.text = "Missing input(s)";
        }
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
        }

        var a1Task = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).Child($"{length + 1}").Child("A1").SetValueAsync(_answer1);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (a1Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a1Task.Exception}");
        }

        var a2Task = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).Child($"{length + 1}").Child("A2").SetValueAsync(_answer2);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (a2Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a2Task.Exception}");
        }

        var a3Task = DBreference.Child("Qns").Child($"{worldNumber}").Child($"{sectionNumber}").Child(difficulty).Child($"{length + 1}").Child("A3").SetValueAsync(_answer3);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (a3Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a3Task.Exception}");
        }

        ClearQuestionAndAnswersFields();

        AddQuestionPanel.gameObject.SetActive(false);
        OptionSelectionPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);// shift this to button
    }
    //Method is called when searchbar is filled
    public void SearchStudent()
    {
        StudentSearched = SearchBar.text;
        Debug.Log(StudentSearched);
        SearchBar.text = "";
        StartCoroutine(ShowInformation(StudentSearched));
    }
    //This method is called to display the individual student statistic from searchbar
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
    //Method is called when the filter button is clicked
    public void filter()
    {
        world = Worldselection.options[Worldselection.value].text;
        section = SectionSelection.options[SectionSelection.value].text;
        StartCoroutine(LoadOverallStatistics(world, section));
    }
    //This method creates the entries in the scrollview depending on the filters selected
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
                else if (world == "TOTAL")
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
                Debug.Log(points);
                GameObject scoreboardElement = Instantiate(statisticElement, scoreboardContent);
                scoreboardElement.GetComponent<StatisticElement>().NewScoreElement(username,stars, points);
            }
        }
    }
    //Button to call the display of all the 
    public void DisplayAllQuestionButtonMethod()
    {
        Debug.Log("Reached here!");
        StartCoroutine(LoadAllQuestionsToDisplay());
    }
    private IEnumerator LoadAllQuestionsToDisplay()
    {
        int world = EditQuestion.World;
        int section = EditQuestion.Section;
        string difficulty = EditQuestion.Difficulty;

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
                questionListElement.GetComponent<QuestionItem>().NewQuestionItem(question, world, section);
            }
        }
    }
    public void EditData(string _question)
    {
        Question = _question;
        EditQuestionInputField.text = Question;
        StartCoroutine(GetQuestionData());
        EditQuestionPanel.gameObject.SetActive(true);
        DisplayQuestionPanel.gameObject.SetActive(false);
    }
    private IEnumerator GetQuestionData() { 
        int world = EditQuestion.World;
        int section = EditQuestion.Section;
        string difficulty = EditQuestion.Difficulty;

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
                totalquestion = totalquestion + 1;
                if (question == Question)
                {
                    string Answer1 = childSnapshot.Child("A1").Value.ToString();
                    string Answer2 = childSnapshot.Child("A2").Value.ToString();
                    string Answer3 = childSnapshot.Child("A3").Value.ToString();
                    EditAnswerInputField1.text = Answer1;
                    EditAnswerInputField2.text = Answer2;
                    EditAnswerInputField3.text = Answer3;
                    currentindex = totalquestion;
                }
             }
        }
    }
    public void savenewquestion()
    {
        string revisedquestion = EditQuestionInputField.text;
        string revisedA1 = EditAnswerInputField1.text;
        string revisedA2 = EditAnswerInputField2.text;
        string revisedA3 = EditAnswerInputField3.text;
        StartCoroutine(setrevisedquestion(revisedquestion,revisedA1,revisedA2,revisedA3));
    }
    private IEnumerator setrevisedquestion(string _question, string _answer1, string _answer2, string _answer3)
    {
        //need integrate with jh one!
        int index = totalquestion - currentindex + 1;
        int world = EditQuestion.World;
        int section = EditQuestion.Section;
        string difficulty = EditQuestion.Difficulty;


        var qnTask = DBreference.Child("Qns").Child($"{world}").Child($"{section}").Child(difficulty).Child($"{index}").Child("Question").SetValueAsync(_question);

        yield return new WaitUntil(predicate: () => qnTask.IsCompleted);

        if (qnTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {qnTask.Exception}");
        }

        var a1Task = DBreference.Child("Qns").Child($"{world}").Child($"{section}").Child(difficulty).Child($"{index}").Child("A1").SetValueAsync(_answer1);

        yield return new WaitUntil(predicate: () => a1Task.IsCompleted);

        if (a1Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a1Task.Exception}");
        }

        var a2Task = DBreference.Child("Qns").Child($"{world}").Child($"{section}").Child(difficulty).Child($"{index}").Child("A2").SetValueAsync(_answer2);

        yield return new WaitUntil(predicate: () => a2Task.IsCompleted);

        if (a2Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a2Task.Exception}");
        }

        var a3Task = DBreference.Child("Qns").Child($"{world}").Child($"{section}").Child(difficulty).Child($"{index}").Child("A3").SetValueAsync(_answer3);

        yield return new WaitUntil(predicate: () => a3Task.IsCompleted);

        if (a3Task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {a3Task.Exception}");
        }

        //ClearQuestionAndAnswersFields();

        EditQuestionPanel.gameObject.SetActive(false);
        FunctionSelectionPanel.gameObject.SetActive(true);// shift this to button
    }
    public void checkRoomExist(){
        StartCoroutine(CheckExistingRoom());
    }
    private IEnumerator CheckExistingRoom(){
        Room = createRoomID.text;
        if (Room == "")
        {
            errormsg.text = "Please input something.";
        }
        else{
            var DBTask = DBreference.Child("custom").GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
            Debug.Log("here");
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
    public void displayallquestions(){
        Debug.Log("Yes");
        StartCoroutine(filterquestions());
    }
    public IEnumerator filterquestions(){
        int world = FilterAssignment.World;
        int section = FilterAssignment.Section;
        string difficulty = FilterAssignment.Difficulty;

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
            foreach (Transform child in CustomquestionListContent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string question = childSnapshot.Child("Question").Value.ToString();
                Debug.Log(question);
                GameObject CustomquestionListElement = Instantiate(CustomquestionElement, CustomquestionListContent);
                CustomquestionListElement.GetComponent<QuestionListItem>().NewQuestionItem(question);
            }
        }
    }
    public void newRoom()
    {
        questionNo = 0;
    }
    public void Addquestion(string _question){
        CustomQuestion = _question;
        Debug.Log(Question);
        StartCoroutine(LoadQuestionAndAnswer());
        StartCoroutine(PushtoDB());
    }
    public IEnumerator LoadQuestionAndAnswer(){
        int world = FilterAssignment.World;
        int section = FilterAssignment.Section;
        string difficulty = FilterAssignment.Difficulty;

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
                if (question == CustomQuestion)
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
        int world = FilterAssignment.World;
        int section = FilterAssignment.Section;
        string difficulty = FilterAssignment.Difficulty;
        questionNo = questionNo + 1;

        var qnTask = DBreference.Child("custom").Child(Room).Child($"{questionNo}").Child("Question").SetValueAsync(CustomQuestion);

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
        var aTask = DBreference.Child("teachers").Child(User.UserId).GetValueAsync();
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
    public void checkRoomExistForViewScore(){
        StartCoroutine(CheckExistingRoomView());
    }
    private IEnumerator CheckExistingRoomView(){
        Room = ViewRoomID.text;
        //Debug.Log(Room);
        var DBTask = DBreference.Child("custom").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        Debug.Log("here");
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
                Debug.Log(roomid);
                if (roomid == Room) {
                    Debug.Log("i did it");
                    ScorePanel.gameObject.SetActive(true);
                    StartPanel.gameObject.SetActive(false);
                    roomexist = true;
                    errormsg1.text = "";
                }
            }
            if (roomexist != true)
            {
                errormsg1.text = "Room does not exist!";
            }
        }
    }
    public void displayStudentScores(){
        Debug.Log("Yes");
        StartCoroutine(StudentScore());
    }
    public IEnumerator StudentScore(){
        Room = ViewRoomID.text;

         var DBTask = DBreference.Child("custom").Child(Room).Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log("Yea");
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            foreach (Transform child in studentListContent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string score = childSnapshot.Value.ToString();
                string user = childSnapshot.Key.ToString();
                GameObject studentListElement = Instantiate(studentElement, studentListContent);
                studentListElement.GetComponent<StudentScoreItem>().CreateItem(user, score);
            }
        }
    }

    //Function for the scoreboard button
    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }

    private IEnumerator LoadScoreboardData()
    {
        int rank = 0;
        string worldsection = "All";

        //Get all the users data ordered by kills amount
        var DBTask = DBreference.Child("users").OrderByChild("TotalPoints").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in leaderboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                rank += 1;
                string username = childSnapshot.Child("username").Value.ToString();
                int points = int.Parse(childSnapshot.Child("TotalPoints").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(leaderElement, leaderboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement($"{rank}", username, worldsection, points);
            }

            //Go to scoreboard screen
            //UIcontroller.instance.PressLeaderboardButton();
        }
    }

    public void displayWorldSectionData()
    {
        StartCoroutine(LoadWorldSectionData());
    }

    private IEnumerator LoadWorldSectionData()
    {
        int rank = 0;
        int worldNumber = UIcontroller.WorldLdrboard;
        int sectionNumber = UIcontroller.SectionLdrboard;
        Debug.Log(worldNumber);
        Debug.Log(sectionNumber);
        //Get all the users data ordered by points
        var DBTask = DBreference.Child("users").OrderByChild("BattleStats/" + $"{worldNumber}" + "/" + $"{sectionNumber}" + "/Points").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in leaderboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                rank += 1;
                string username = childSnapshot.Child("username").Value.ToString();
                Debug.Log(username);
                int points = int.Parse(childSnapshot.Child("BattleStats").Child($"{worldNumber}").Child($"{sectionNumber}").Child("Points").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(leaderElement, leaderboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement($"{rank}", username, $"{worldNumber}" + "." + $"{sectionNumber}", points);
            }
        }
    }
}
    


///// FIX ERROR HANDLING!!!!!