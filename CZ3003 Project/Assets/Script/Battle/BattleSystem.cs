using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;



public enum BattleState { Start, ActionSelection, MoveSelection, PlayerAnswer, PerformMove, Busy, BattleOver }

public class BattleSystem : MonoBehaviour
{
    //[SerializeField] BattleUnit playerUnit;
    //[SerializeField] BattleUnit enemyUnit;
    //[SerializeField] TrainerController trainer;
    [SerializeField] PlayerController player;
    // [SerializeField] BattleHud playerHud;
    // [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] BattleQuestions battleQuestions;
    BattleUnit trainerUnit;

    // BattleSystem battleSystem;

    public static BattleSystem Instance{ get; private set; }

    public void Awake() {
        Instance = this;
    }

    public event Action<bool> onBattleOver;

    BattleState state;

    int currentAction;
    int currentMove;
    int currentAnswer;
    int correctAnswer;
    bool isCorrect = true;

    public void StartBattle(BattleUnit trainerUnit) {
        StartCoroutine(SetupBattle(trainerUnit));
    }
    // i changed all enemyunit to trainer.TrainerUnit and playerunit to player.PlayerUnit
    public IEnumerator SetupBattle(BattleUnit trainerUnit) {
        this.trainerUnit = trainerUnit;
        player.PlayerUnit.SetUp();
        trainerUnit.SetUp();
        //trainer.TrainerUnit.SetUp();
        // playerUnit.SetUp();
        // enemyUnit.SetUp();
        dialogBox.SetMoveNames();
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

    public void BattleOver(bool won) {
        state = BattleState.BattleOver; //notifies state only
        onBattleOver(won); //onBattleOver notifies gamecontroller that its over
    }

    public void ActionSelection() {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
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
            StartCoroutine(EnemyMove());
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
        else
            BattleOver(true); 
    }

    public void HandleUpdate() {
        if (state == BattleState.ActionSelection) {
            HandleActionSelection();
        }

        else if (state == BattleState.MoveSelection) {
            HandleMoveSelection();
        }
        else if (state == BattleState.PlayerAnswer) {
            HandleAnswerSelection(correctAnswer);
        }

    }

    public void HandleActionSelection() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentAction < 1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
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
            }
        }
    }


    public void HandleMoveSelection() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentMove < 2)//playerUnit.Monster.Moves.Count - 1
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (currentMove > 0)
                --currentMove;
        } 

        dialogBox.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (currentMove == 0) {
                dialogBox.EnableQuestionText(true);
                dialogBox.EnableAnswerSelector(true);
                StartCoroutine(dialogBox.TypeQuestion(SelectQuestion(battleQuestions.Questions.QB, "Easy").Question));
                correctAnswer = dialogBox.SetAnswer(SelectQuestion(battleQuestions.Questions.QB, "Easy"));
                PlayerAnswer();
            }
            else if (currentMove == 1) {
                dialogBox.EnableQuestionText(true);
                dialogBox.EnableAnswerSelector(true);
                StartCoroutine(dialogBox.TypeQuestion(SelectQuestion(battleQuestions.Questions.QB, "Medium").Question));
                correctAnswer = dialogBox.SetAnswer(SelectQuestion(battleQuestions.Questions.QB, "Medium"));
                PlayerAnswer();
            }
            else if (currentMove == 2) {
                dialogBox.EnableQuestionText(true);
                dialogBox.EnableAnswerSelector(true);
                StartCoroutine(dialogBox.TypeQuestion(SelectQuestion(battleQuestions.Questions.QB, "Hard").Question));
                correctAnswer = dialogBox.SetAnswer(SelectQuestion(battleQuestions.Questions.QB, "Hard"));
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
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentAnswer < 2)
                ++currentAnswer;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
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
                StartCoroutine(EnemyMove());
            }
        }

    }
    
}
