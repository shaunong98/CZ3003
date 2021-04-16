// Authors: Jethro
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
//using UnityEngine.UI;

// Creating BattleStates during the Battle System to instantiate different actions.
public enum CustomBattleState { Start, ActionSelection, MoveSelection, PlayerAnswer, PerformMove, Busy, BattleOver }

public class CustomBattleSystem : MonoBehaviour
{

    [SerializeField] private AudioClip arrowClickSFX;
    [SerializeField] private AudioClip cfmClickSFX;

    [SerializeField] private AudioClip AttackMusic;
    [SerializeField] private AudioClip NoHealthMusic;

    [SerializeField] private AudioClip runMusic;

    // Reference to player controller.
    [SerializeField] public PlayerController player;

    // Reference to battle dialogbox to access its methods.
    [SerializeField] BattleDialogBox dialogBox;

    // Reference to trainer unit
    BattleUnit trainerUnit;

    // Bool variable to check if the battle system is pvp or not.
    public bool isCustom;

    // Creating an instance of the custombattlesystem so that we will be able to reference it from another class.
    public static CustomBattleSystem Instance{ get; private set; }

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public DatabaseReference DBreference;

    // Instantiating an event for other classes to subscribe to it.
    public event Action<bool> onBattleOver;

    // Creating a variable of state to reference to it.
    CustomBattleState state;

    // Current Action refers to where the player cursor is pointing to -  "Fight" or "Run".
    int currentAction;

    // Current Answer refers to where the player cursor is pointing to -  "A1" or "A2" or "A3".
    int currentAnswer;

    // Current Answer refers to where the player cursor is pointing to -  "A1" or "A2" or "A3".
    public static int correctAnswer;

    // Question num to record which question he/she is at right now.
    public static int questionNum = 1;

    // TotalQuestionNum to store the total number of questions that is given in the custom room.
    public static int totalQuestionNum = 1;

    // Total correct answer stores the total number of correct answers.
    public static int totalcorrectAnswer = 0;

    // Boolean variable to see if the student has scored correct or not.
    bool isCorrect = true;

    // Awake method to be called when custombattlesystem is instantiated.
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

    // Method to initializefirebase.
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Method to start the coroutine for the custom battle scene.
    public void StartBattle(BattleUnit trainerUnit) {
        StartCoroutine(SetupBattle(trainerUnit));

    }

    // Coroutine to set up the battle scene.
    public IEnumerator SetupBattle(BattleUnit trainerUnit) {
        this.trainerUnit = trainerUnit;
        player.PlayerUnit.SetUp(true);
        trainerUnit.SetUp(false);    
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
        if (!isCustom) {
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

    // Method that calls the state BattleOver and updates the player score into the firebase.
    public void BattleOver(bool won) { //if true means player has won
        state = CustomBattleState.BattleOver; 
        StartCoroutine(updateUserCustomBattlePoints(won));
        Debug.Log("How to solve this");
    }

    // Coroutine to update players score to firebase.
    public IEnumerator updateUserCustomBattlePoints(bool won)
    {
        FirebaseUser User;
        User = FirebaseManager.User;
        Debug.Log("update user battle");
        Debug.Log(User.UserId);
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

    // Method to call the state action selection. 
    public void ActionSelection() {
        state = CustomBattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    // Method to call the state action selection if the player has answered wrongly (for custom) and if the player has answered all questions, the method BattleOver will be called.
    public IEnumerator ActionSelectionifWrong() {
        state = CustomBattleState.ActionSelection;
        yield return dialogBox.TypeDialog("You answered wrongly!");

        if (questionNum == (totalQuestionNum + 1))
        {
            Debug.Log("questionnum = total question");
            yield return dialogBox.TypeDialog("You have answered all questions!");
            yield return dialogBox.TypeDialog($"You have scored a total of {totalcorrectAnswer}/{totalQuestionNum} correct!");
            yield return new WaitForSeconds(1f);
            BattleOver(false);
        }
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    // Method to call the state answer selection. 
    public void PlayerAnswer() {
        state = CustomBattleState.PlayerAnswer;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableQuestionText(true);
        dialogBox.EnableAnswerSelector(true);
        dialogBox.EnableMoveSelector(false);
    }

    // Method to call the move made by the player. 
    public IEnumerator PlayerMove() {
        state = CustomBattleState.PerformMove;
        Move move;
        move = player.PlayerUnit.Monster.Moves[0];
        yield return dialogBox.TypeDialog($"You answered correctly!");
        yield return new WaitForSeconds(1f);

        yield return RunMove(player.PlayerUnit, trainerUnit, move);
       
        if (state == CustomBattleState.PerformMove){
            ActionSelection();
        }
    }

    // Method to call the move made by the players/trainer after the player has answered a question.
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
        AudioManager.Instance.PlaySFX(AttackMusic);
        yield return new WaitForSeconds(1f);

        targetUnit.PlayerHitAnimation();

        bool isFainted = targetUnit.Monster.TakeDamage(move);
        yield return new WaitForSeconds(1f);
        yield return targetUnit.Hud.UpdateHP();

        if (isFainted) {
            yield return dialogBox.TypeDialog($"{targetUnit.Monster.Base.Name} has fainted.");
            targetUnit.PlayerFaintAnimation();
            AudioManager.Instance.PlaySFX(NoHealthMusic);
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

    // Method to check if the battle is over after every turn of fighting.
    public void CheckForBattleOver(BattleUnit faintedUnit) {
        if (faintedUnit.IsPlayerUnit) {
            BattleOver(false);
        }
        else if (isCustom && !faintedUnit.IsPlayerUnit) {
            Debug.Log("completed level");
            dialogBox.completedLevel();
            BattleOver(true);
        }
        else
            BattleOver(true); 
    }

    // Method to handle all updates on every frames that will be called in CustomController class.
    public void HandleUpdate() {
        if (state == CustomBattleState.ActionSelection) {
            HandleActionSelection();
            
        }
        else if (state == CustomBattleState.PlayerAnswer) {
            HandleAnswerSelection(correctAnswer);
            
        }

    }

    // Method to handle action selection and display questions & answers from the firebase
    public void HandleActionSelection() {
        if (Input.GetKeyDown(KeyCode.S)) {
            AudioManager.Instance.PlaySFX(arrowClickSFX);
            if (currentAction < 1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.W)) {
            AudioManager.Instance.PlaySFX(arrowClickSFX);
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space)) {
            AudioManager.Instance.PlaySFX(cfmClickSFX);
            if (currentAction == 0) {
                dialogBox.EnableQuestionText(true);
                Debug.Log("enable question is selected");
                dialogBox.EnableAnswerSelector(false);
                dialogBox.RestartAnswerSelection();

                StartCoroutine(QuestionManager.Instance.getQuestionsforCustom(questionNum));
                Debug.Log($"correct answer is {correctAnswer}");
                PlayerAnswer();
            }
            else if (currentAction == 1) {
                AudioManager.Instance.PlaySFX(runMusic);
                BattleOver(false);
            }
        }
    }

    // Method to handle answer selection as you choose a different answer.
    public void HandleAnswerSelection(int answer) {
        if (Input.GetKeyDown(KeyCode.D)) {
            AudioManager.Instance.PlaySFX(arrowClickSFX);
            if (currentAnswer < 2)
                ++currentAnswer;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            AudioManager.Instance.PlaySFX(arrowClickSFX);
            if (currentAnswer > 0)
                --currentAnswer;
        }

        dialogBox.UpdateAnswerSelection(currentAnswer);

        if (Input.GetKeyDown(KeyCode.Space)) {
            AudioManager.Instance.PlaySFX(cfmClickSFX);
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
