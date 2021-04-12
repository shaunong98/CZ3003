using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
//using UnityEngine.UI;

// Creating BattleStates during the Battle System to instantiate different actions.
public enum BattleState { Start, ActionSelection, MoveSelection, PlayerAnswer, PerformMove, Busy, BattleOver }

public class BattleSystem : MonoBehaviour
{
    // Reference to player controller.
    [SerializeField] public PlayerController player;

    [SerializeField] private AudioClip arrowClickSFX;
    [SerializeField] private AudioClip cfmClickSFX;

    [SerializeField] private AudioClip AttackMusic;
    [SerializeField] private AudioClip NoHealthMusic;

    [SerializeField] private AudioClip runMusic;

    // Reference to battle dialogbox to access its methods.
    [SerializeField] BattleDialogBox dialogBox;

    // Reference to trainer unit however this variable will be passed by the GameController class.
    BattleUnit trainerUnit;

    // Bool variable to check if the battle system is pvp or not.
    public bool isPVP;

    // Creating an instance of the battlesystem so that we will be able to reference it from another class.
    public static BattleSystem Instance{ get; private set; }

    // Instantiating an event for other classes to subscribe to it.
    public event Action<bool> onBattleOver;

    // Creating a variable of state to reference to it.
    BattleState state;

    // Current Action refers to where the player cursor is pointing to -  "Fight" or "Run".
    int currentAction;

    // Current Move refers to where the player cursor is pointing to -  "Easy" or "Medium" or "Hard".
    int currentMove;

    // Current Answer refers to where the player cursor is pointing to -  "A1" or "A2" or "A3".
    int currentAnswer;

    // Index of the correct answer.
    public static int correctAnswer;

    // bool variable to store if player answered correctly or not.
    bool isCorrect = true;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public DatabaseReference DBreference;

    // Awake function to call whenever this class is called.
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

    // Initializing firebase.
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

        // Method to call the state action selection. 
    public void ActionSelection() {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    // Method to call the state action selection if the player has answered wrongly (for pvp). 
    public void ActionSelectionifWrong() {
        state = BattleState.ActionSelection;
        //isCorrect = true;
        StartCoroutine(dialogBox.TypeDialog("You answered wrongly! Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    // Method to call the state move selection. 
    public void MoveSelection() {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableAnswerSelector(false);
        dialogBox.EnableMoveSelector(true);
        dialogBox.EnableQuestionText(false);
    }

    // Method to call the state answer selection. 
    public void PlayerAnswer() {
        state = BattleState.PlayerAnswer;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableQuestionText(true);
        dialogBox.EnableAnswerSelector(true);
        dialogBox.EnableMoveSelector(false);
    }

    // Method to start the coroutine for the setting up of battle.
    public void StartBattle(BattleUnit trainerUnit) {
        StartCoroutine(SetupBattle(trainerUnit));
    }

    // Method to set up the battlescene.
    public IEnumerator SetupBattle(BattleUnit trainerUnit) {
        this.trainerUnit = trainerUnit;
        player.PlayerUnit.SetUp(true);
        trainerUnit.SetUp(false);    
        //trainer.TrainerUnit.SetUp();
        // playerUnit.SetUp();
        // enemyUnit.SetUp();
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
        yield return dialogBox.TypeDialog($"{trainerUnit.Monster.Base.Name} has challenged you to a duel."); //use startcoroutine because its ienumerator
        yield return new WaitForSeconds(1f);
        dialogBox.EnableActionSelector(true);
        ActionSelection();
    }

    // Params won - True: Player has won; False: Player has lost.
    // Method to call the event that battle is over to signal to the controllers that the battle has ended.
    public void BattleOver(bool won) { //if true means player has won
        state = BattleState.BattleOver; //notifies state only
        if (won && isPVP) {
            Debug.Log($"{dialogBox.Points}");
            StartCoroutine(updateUserBattlePoints(dialogBox.Points, won));
            Debug.Log("How to solve this");
        }
        else {
            onBattleOver(won);
        }
    }

    // Params won - True: Player has won; False: Player has lost.
    // Params points - represent points earned by the player.
    // Updating the Firebase of the total points obtained by the player while making sure that its the highest points obtained.
    public IEnumerator updateUserBattlePoints(int points, bool won)
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
        //string initialPoint;
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
            if (points > initialPoints)
            {
                var DBTasks = DBreference.Child("users").Child(User.UserId).Child("BattleStats").Child($"{worldNumber}").Child($"{sectionNumber}").Child("Points").SetValueAsync(points);
                yield return new WaitUntil(predicate: () => DBTasks.IsCompleted);

                if (DBTasks.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTasks.Exception}");
                }
                else
                {
                    //points is now updated
                }

                var DBTask1 = DBreference.Child("users").Child(User.UserId).GetValueAsync();
                yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
                Debug.Log("reached here at users");
                if (DBTask1.Exception != null)
                {
                    Debug.Log("hello");
                    Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                }
                else if (DBTask1.Result.Value == null)
                {
                    Debug.Log("what");
                }
                else
                {
                    DataSnapshot snapshot1 = DBTask1.Result;
                    string totalPoint = snapshot1.Child("TotalPoints").Value.ToString();
                    int totalPoints = int.Parse(totalPoint);

                    totalPoints = totalPoints - initialPoints + points;
                    Debug.Log($"{totalPoints}");

                    var DBTask2 = DBreference.Child("users").Child(User.UserId).Child("TotalPoints").SetValueAsync(totalPoints);
                    yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

                    if (DBTask2.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {DBTasks.Exception}");
                    }
                    else
                    {
                        //total points is now updated
                    }
                }
            }
        }
        onBattleOver(won);
    }    

    // Method to call the move made by the player. 
    public IEnumerator PlayerMove() {
        state = BattleState.PerformMove;
        Move move;
        if (currentMove == 2) {
           move = player.PlayerUnit.Monster.Moves[currentMove-1];
        }
        else if (currentMove == 1) {
           move = player.PlayerUnit.Monster.Moves[currentMove+1];
        }
        else {move = player.PlayerUnit.Monster.Moves[currentMove];}
        yield return dialogBox.TypeDialog($"You answered correctly!");
        yield return new WaitForSeconds(1f);

        yield return RunMove(player.PlayerUnit, trainerUnit, move);
       
        if (state == BattleState.PerformMove){
            if (!isPVP) {
                StartCoroutine(EnemyMove()); //not sure this part
            }
            else {
                ActionSelection();
            }
        }
    }

    // Method to call the move made by the trainer. 
    public IEnumerator EnemyMove() {
        state = BattleState.PerformMove;

        var move = trainerUnit.Monster.GetRandomMove();
        if (!isCorrect) {
            yield return dialogBox.TypeDialog("You answered wrongly!");
            yield return new WaitForSeconds(1f);
            isCorrect = true;
        }
        yield return RunMove(trainerUnit, player.PlayerUnit, move);

        if (state == BattleState.PerformMove) {
            ActionSelection();
        }
    }

    // Method to call the move made by the players/trainer after the player has answered a question.
    public IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move) {
        FirebaseUser User;
        User = FirebaseManager.User;
        string username = "";
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
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
        if (sourceUnit.IsPlayerUnit) {
            yield return dialogBox.TypeDialog($"{username} used {move.Base.Name}.");
        }
        else {
            yield return dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} used {move.Base.Name}.");
        }
        //yield return dialogBox.TypeDialog($"{username} used {move.Base.Name}.");
        sourceUnit.PlayerAttackAnimation();
        AudioManager.Instance.PlaySFX(AttackMusic);
        yield return new WaitForSeconds(1f);

        targetUnit.PlayerHitAnimation();

        bool isFainted = targetUnit.Monster.TakeDamage(move);
        yield return new WaitForSeconds(1f);
        yield return targetUnit.Hud.UpdateHP();

        if (isFainted) {
            if (targetUnit.IsPlayerUnit) {
                yield return dialogBox.TypeDialog($"{username} has fainted.");
            }
            else {
                yield return dialogBox.TypeDialog($"{targetUnit.Monster.Base.Name} has fainted.");
            }
            targetUnit.PlayerFaintAnimation();
            AudioManager.Instance.PlaySFX(NoHealthMusic);
            yield return new WaitForSeconds(2f);
            CheckForBattleOver(targetUnit);
        }
    }

    // Method to check if the battle is over after every turn of fighting.
    public void CheckForBattleOver(BattleUnit faintedUnit) {
        if (faintedUnit.IsPlayerUnit) {
            BattleOver(false);
        }
        else if (isPVP && !faintedUnit.IsPlayerUnit) {
            dialogBox.completedLevel();
            BattleOver(true);
        }
        else
            BattleOver(true); 
    }

    // Method to Handle all updates every frames that will be called in GameController class.
    public void HandleUpdate() {
        if (state == BattleState.ActionSelection) {
            HandleActionSelection();
            if (isPVP) {
                HandleTimer();
            }
        }

        else if (state == BattleState.MoveSelection) {
            HandleMoveSelection();
            if (isPVP) {
                HandleTimer();
            }
        }
        else if (state == BattleState.PlayerAnswer) {
            HandleAnswerSelection(correctAnswer);
            if (isPVP) {
                HandleTimer();
            }
        }

    }

    // Method to check if the timer has reached 0 - battle over and to reduce time by 10 seconds for every incorrect answer.
    public void HandleTimer() 
    {
        if (!isCorrect) {
            dialogBox.Timer -= 10f;
            isCorrect = true;
        }
        if (!dialogBox.TimerPaused)
        {
            dialogBox.Timer -= Time.deltaTime;
        }
        dialogBox.SetTimer(dialogBox.Timer.ToString());
        if (dialogBox.Timer <= 0)
        {
            BattleOver(false); //player have lost.
        }
    }
    
    // Method to handle action selection as you choose a different answer.
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
                MoveSelection();
            }
            else if (currentAction == 1) {
                //run
                AudioManager.Instance.PlaySFX(runMusic);
                BattleOver(false);
            }
        }
    }

    // Method to handle move selection as you choose a different answer and display questions & answers from the firebase
    public void HandleMoveSelection() {
        
        if (Input.GetKeyDown(KeyCode.D)) {
            AudioManager.Instance.PlaySFX(arrowClickSFX);            
            if (currentMove < 2)//playerUnit.Monster.Moves.Count - 1
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            AudioManager.Instance.PlaySFX(arrowClickSFX);
            if (currentMove > 0)
                --currentMove;
        } 

        dialogBox.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Space)) {
            AudioManager.Instance.PlaySFX(cfmClickSFX);
            if (currentMove == 0) {
                dialogBox.EnableQuestionText(true);
                Debug.Log("enable question is selected");
                dialogBox.EnableAnswerSelector(false);
                dialogBox.RestartAnswerSelection();
                StartCoroutine(QuestionManager.Instance.getQuestionsBaseOnLevel("Easy"));
                Debug.Log($"correct answer is {correctAnswer}");
                PlayerAnswer();
            }
            else if (currentMove == 1) {
                dialogBox.EnableQuestionText(true);
                dialogBox.EnableAnswerSelector(true);
                dialogBox.RestartAnswerSelection();
                StartCoroutine(QuestionManager.Instance.getQuestionsBaseOnLevel("Medium"));
                Debug.Log($"correct answer is {correctAnswer}");
                PlayerAnswer();
            }
            else if (currentMove == 2) {
                dialogBox.EnableQuestionText(true);
                dialogBox.EnableAnswerSelector(true);
                dialogBox.RestartAnswerSelection();
                StartCoroutine(QuestionManager.Instance.getQuestionsBaseOnLevel("Hard"));
                Debug.Log($"correct answer is {correctAnswer}");
                PlayerAnswer();
            }

        }
    }

    // Method to handle answer selection as you choose a different answer.
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
                StartCoroutine(PlayerMove());

            }
            else if (currentAnswer == 1 && currentAnswer == answer) {
                dialogBox.EnableDialogText(true);
                dialogBox.EnableAnswerSelector(false);
                dialogBox.EnableQuestionText(false);
                StartCoroutine(PlayerMove());
            }
            else if (currentAnswer == 2 && currentAnswer == answer) {
                dialogBox.EnableDialogText(true);
                dialogBox.EnableAnswerSelector(false);
                dialogBox.EnableQuestionText(false);
                StartCoroutine(PlayerMove());
            }
            else {
                dialogBox.EnableDialogText(true);
                dialogBox.EnableAnswerSelector(false);
                dialogBox.EnableQuestionText(false);
                isCorrect = false;
                if (!isPVP) {
                    StartCoroutine(EnemyMove());
                }
                else {
                    ActionSelectionifWrong();
                }
                
            }
        }

    }
    
}
