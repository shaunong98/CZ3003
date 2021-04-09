using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
//using UnityEngine.UI;


public enum CustomBattleState { Start, ActionSelection, MoveSelection, PlayerAnswer, PerformMove, Busy, BattleOver }

public class CustomBattleSystem : MonoBehaviour
{
    //[SerializeField] BattleUnit playerUnit;
    //[SerializeField] BattleUnit enemyUnit;
    //[SerializeField] TrainerController trainer;
    [SerializeField] public PlayerController player;
    // [SerializeField] BattleHud playerHud;
    // [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    //[SerializeField] CountdownTimer countDown;
    BattleUnit trainerUnit;
    public bool isPVP;

    // BattleSystem battleSystem;

    public static CustomBattleSystem Instance{ get; private set; }

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    //public FirebaseUser User;
    public DatabaseReference DBreference;

    public event Action<bool> onBattleOver;

    CustomBattleState state;

    int currentAction;
    int currentMove = 0;
    int currentAnswer;
    public static int correctAnswer;
    public static int questionNum = 1;
    public static int totalQuestionNum = 1;
    public static int totalcorrectAnswer = 0;
    bool isCorrect = true;

    public void Awake() {
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
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void StartBattle(BattleUnit trainerUnit) {
        StartCoroutine(SetupBattle(trainerUnit));

    }
    // i changed all enemyunit to trainer.TrainerUnit and playerunit to player.PlayerUnit
    public IEnumerator SetupBattle(BattleUnit trainerUnit) {
        this.trainerUnit = trainerUnit;
        player.PlayerUnit.SetUp(true);
        trainerUnit.SetUp(false);    
        //string initialPoint;
        string username = "";
        var DBTask = DBreference.Child("custom").Child(QuestionManager.roomID).GetValueAsync();
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
            username = snapshot.Child("usercreated").Value.ToString();
        }

        dialogBox.SetMoveNames();
        if (!isPVP) {
            Debug.Log("timer text true");
            dialogBox.EnableTimerText(false);
        }
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableQuestionText(false);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
        yield return dialogBox.TypeDialog($"{username} has challenged you to a duel."); // chnge this to access custom battlesystem to include username of challanger
        yield return new WaitForSeconds(1f);
        dialogBox.EnableActionSelector(true);
        ActionSelection();
    }

    public void BattleOver(bool won) { //if true means player has won
        state = CustomBattleState.BattleOver; //notifies state only
        //StudentFireBase.Instance.updateBattlePoints(dialogBox.Points);
        StartCoroutine(updateUserCustomBattlePoints(won));
        Debug.Log("How to solve this");
    }

    public IEnumerator updateUserCustomBattlePoints(bool won)
    {
        //need add in username and the score for this
        FirebaseUser User;
        User = FirebaseManager.User;
        Debug.Log("update user battle");
        Debug.Log(User.UserId);
        //string initialPoint;
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
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
            string username = snapshot.Child("username").Value.ToString();
           
            var DBTasks = DBreference.Child("custom").Child(QuestionManager.roomID).Child("users").Child(username).SetValueAsync($"{totalcorrectAnswer}/{totalQuestionNum}");
            yield return new WaitUntil(predicate: () => DBTasks.IsCompleted);

            if (DBTasks.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTasks.Exception}");
            }
            else
            {
                //points is now updated
            }
        }
        onBattleOver(won);
    }    

    public void ActionSelection() {
        state = CustomBattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    public IEnumerator ActionSelectionifWrong() {
        state = CustomBattleState.ActionSelection;
        //isCorrect = true;
        yield return dialogBox.TypeDialog("You answered wrongly!");

        if (questionNum == (totalQuestionNum + 1))
        {
            Debug.Log("questionnum = total question");
            yield return dialogBox.TypeDialog("You have answered all questions!");
            yield return dialogBox.TypeDialog($"You have scored a total of {totalcorrectAnswer}/{totalQuestionNum} correct!");
            yield return new WaitForSeconds(1f);
            BattleOver(false);
        }
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    public void PlayerAnswer() {
        state = CustomBattleState.PlayerAnswer;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableQuestionText(true);
        dialogBox.EnableAnswerSelector(true);
        dialogBox.EnableMoveSelector(false);
    }

    public IEnumerator PlayerMove() {
        state = CustomBattleState.PerformMove;
        Move move;
        move = player.PlayerUnit.Monster.Moves[currentMove];
        yield return dialogBox.TypeDialog($"You answered correctly!");
        yield return new WaitForSeconds(1f);

        yield return RunMove(player.PlayerUnit, trainerUnit, move);
       
        if (state == CustomBattleState.PerformMove){
            ActionSelection();
        }
    }

    public IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move) {
        FirebaseUser User;
        User = FirebaseManager.User;
        Debug.Log("update user battle");
        Debug.Log(User.UserId);
        string username = "";
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
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
            username = snapshot.Child("username").Value.ToString();
        }
        yield return dialogBox.TypeDialog($"{username} used {move.Base.Name}.");
        sourceUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(1f);

        targetUnit.PlayerHitAnimation();

        bool isFainted = targetUnit.Monster.TakeDamage(move);
        yield return new WaitForSeconds(1f);
        yield return targetUnit.Hud.UpdateHP();

        if (isFainted) {
            yield return dialogBox.TypeDialog($"{targetUnit.Monster.Base.Name} has fainted.");
            targetUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(2f);
            CheckForBattleOver(targetUnit);
        }
        Debug.Log($"{questionNum}");
        Debug.Log($"{totalQuestionNum}");
        if (questionNum == (totalQuestionNum + 1))
        {
            yield return dialogBox.TypeDialog("You have answered all questions!");
            yield return dialogBox.TypeDialog($"You have scored a total of {totalcorrectAnswer}/{totalQuestionNum} correct!");
            yield return new WaitForSeconds(1f);
            BattleOver(false);
        }
        
    }

    public void CheckForBattleOver(BattleUnit faintedUnit) {
        if (faintedUnit.IsPlayerUnit) {
            BattleOver(false);
        }
        else if (isPVP && !faintedUnit.IsPlayerUnit) {
            Debug.Log("completed level");
            dialogBox.completedLevel();
            BattleOver(true);
        }
        else
            BattleOver(true); 
    }

    public void HandleUpdate() {
        if (state == CustomBattleState.ActionSelection) {
            HandleActionSelection();
            
        }

        else if (state == CustomBattleState.PlayerAnswer) {
            HandleAnswerSelection(correctAnswer);
            
        }

    }

    public void HandleActionSelection() {
        if (Input.GetKeyDown(KeyCode.S)) {
            if (currentAction < 1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.W)) {
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (currentAction == 0) {
                dialogBox.EnableQuestionText(true);
                Debug.Log("enable question is selected");
                dialogBox.EnableAnswerSelector(false);
                dialogBox.RestartAnswerSelection();
                //StartCoroutine(dialogBox.TypeQuestion(SelectQuestion(battleQuestions.Questions.QB, "Easy").Question));
                StartCoroutine(QuestionManager.Instance.getQuestionsforCustom(questionNum));
                Debug.Log($"correct answer is {correctAnswer}");
                PlayerAnswer();
            }
            else if (currentAction == 1) {
                //run
                BattleOver(false);
            }
        }
    }

    public void HandleAnswerSelection(int answer) {
        if (Input.GetKeyDown(KeyCode.D)) {
            if (currentAnswer < 2)
                ++currentAnswer;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            if (currentAnswer > 0)
                --currentAnswer;
        }

        dialogBox.UpdateAnswerSelection(currentAnswer);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (currentAnswer == 0 && currentAnswer == answer) {
                dialogBox.EnableDialogText(true);
                dialogBox.EnableAnswerSelector(false);
                dialogBox.EnableQuestionText(false);
                totalcorrectAnswer++;
                StartCoroutine(PlayerMove());

            }
            else if (currentAnswer == 1 && currentAnswer == answer) {
                dialogBox.EnableDialogText(true);
                dialogBox.EnableAnswerSelector(false);
                dialogBox.EnableQuestionText(false);
                totalcorrectAnswer++;
                StartCoroutine(PlayerMove());
            }
            else if (currentAnswer == 2 && currentAnswer == answer) {
                dialogBox.EnableDialogText(true);
                dialogBox.EnableAnswerSelector(false);
                dialogBox.EnableQuestionText(false);
                totalcorrectAnswer++;
                StartCoroutine(PlayerMove());
            }
            else {
                dialogBox.EnableDialogText(true);
                dialogBox.EnableAnswerSelector(false);
                dialogBox.EnableQuestionText(false);
                isCorrect = false;
                StartCoroutine(ActionSelectionifWrong());
                
            }
        }

    }
    
}
