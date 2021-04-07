using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
//using UnityEngine.UI;


public enum BattleState { Start, ActionSelection, MoveSelection, PlayerAnswer, PerformMove, Busy, BattleOver }

public class BattleSystem : MonoBehaviour
{
    //[SerializeField] BattleUnit playerUnit;
    //[SerializeField] BattleUnit enemyUnit;
    //[SerializeField] TrainerController trainer;
    [SerializeField] public PlayerController player;
    // [SerializeField] BattleHud playerHud;
    // [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] BattleQuestions battleQuestions;
    //[SerializeField] CountdownTimer countDown;
    BattleUnit trainerUnit;
    public bool isPVP;

    // BattleSystem battleSystem;

    public static BattleSystem Instance{ get; private set; }

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    //public FirebaseUser User;
    public DatabaseReference DBreference;

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

    public event Action<bool> onBattleOver;

    BattleState state;

    int currentAction;
    int currentMove;
    int currentAnswer;
    public static int correctAnswer;
    bool isCorrect = true;

    public void StartBattle(BattleUnit trainerUnit) {
        StartCoroutine(SetupBattle(trainerUnit));
    }
    // i changed all enemyunit to trainer.TrainerUnit and playerunit to player.PlayerUnit
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

    public void BattleOver(bool won) { //if true means player has won
        state = BattleState.BattleOver; //notifies state only
        if (won && isPVP) {
            Debug.Log($"{dialogBox.Points}");
            //StudentFireBase.Instance.updateBattlePoints(dialogBox.Points);
            StartCoroutine(updateUserBattlePoints(dialogBox.Points, won));
            Debug.Log("How to solve this");
        }
        else {
            onBattleOver(won);
        }
        //onBattleOver(won); //onBattleOver notifies gamecontroller that its over 
    }

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

    public void ActionSelection() {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    public void ActionSelectionifWrong() {
        state = BattleState.ActionSelection;
        //isCorrect = true;
        StartCoroutine(dialogBox.TypeDialog("You answered wrongly! Choose an action"));
        dialogBox.EnableActionSelector(true);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableAnswerSelector(false);
    }

    public void MoveSelection() {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableAnswerSelector(false);
        dialogBox.EnableMoveSelector(true);
        dialogBox.EnableQuestionText(false);
    }

    public void PlayerAnswer() {
        state = BattleState.PlayerAnswer;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableQuestionText(true);
        dialogBox.EnableAnswerSelector(true);
        dialogBox.EnableMoveSelector(false);
    }

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

    public IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move) {
        yield return dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} used {move.Base.Name}.");
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
            //SceneManager.LoadScene("Map Selection");
            //Application.LoadLevel(levelToLoad);
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
                MoveSelection();
            }
            else if (currentAction == 1) {
                //run
                BattleOver(false);
            }
        }
    }

    public void HandleMoveSelection() {
        
        if (Input.GetKeyDown(KeyCode.D)) {
            if (currentMove < 2)//playerUnit.Monster.Moves.Count - 1
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            if (currentMove > 0)
                --currentMove;
        } 

        dialogBox.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (currentMove == 0) {
                dialogBox.EnableQuestionText(true);
                Debug.Log("enable question is selected");
                dialogBox.EnableAnswerSelector(false);
                dialogBox.RestartAnswerSelection();
                //StartCoroutine(dialogBox.TypeQuestion(SelectQuestion(battleQuestions.Questions.QB, "Easy").Question));
                StartCoroutine(QuestionManager.Instance.getQuestionsBaseOnLevel("Easy"));
                // string question = QuestionManager.Instance.Question;
                // Debug.Log(question);
                // StartCoroutine(dialogBox.TypeQuestion(question));
                //correctAnswer = QuestionManager.correctAnswer;
                Debug.Log($"correct answer is {correctAnswer}");
                //correctAnswer = dialogBox.SetAnswer(SelectQuestion(battleQuestions.Questions.QB, "Easy"));
                PlayerAnswer();
            }
            else if (currentMove == 1) {
                dialogBox.EnableQuestionText(true);
                dialogBox.EnableAnswerSelector(true);
                dialogBox.RestartAnswerSelection();
                //StartCoroutine(dialogBox.TypeQuestion(SelectQuestion(battleQuestions.Questions.QB, "Medium").Question));
                StartCoroutine(QuestionManager.Instance.getQuestionsBaseOnLevel("Medium"));
                Debug.Log($"correct answer is {correctAnswer}");
                //correctAnswer = dialogBox.SetAnswer(SelectQuestion(battleQuestions.Questions.QB, "Medium"));
                PlayerAnswer();
            }
            else if (currentMove == 2) {
                dialogBox.EnableQuestionText(true);
                dialogBox.EnableAnswerSelector(true);
                dialogBox.RestartAnswerSelection();
                //StartCoroutine(dialogBox.TypeQuestion(SelectQuestion(battleQuestions.Questions.QB, "Hard").Question));
                StartCoroutine(QuestionManager.Instance.getQuestionsBaseOnLevel("Hard"));
                Debug.Log($"correct answer is {correctAnswer}");
                //correctAnswer = dialogBox.SetAnswer(SelectQuestion(battleQuestions.Questions.QB, "Hard"));
                PlayerAnswer();
            }

        }
    }

    public QuestionBase SelectQuestion(List<QuestionBase> questions, string difficulty) {
        foreach (var question in questions) {
            if (question.QuestionDifficulty == difficulty) {
                return question;
            }
        }
        return null;
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
                    //dialogBox.TypeDialog("You answered wrongly!");
                    ActionSelectionifWrong();
                }
                
            }
        }

    }
    
}
