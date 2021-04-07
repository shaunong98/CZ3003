using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{FreeRoam, Battle, Dialog, Cutscene}

public class GameController : MonoBehaviour
{
    [SerializeField] public PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    TrainerController trainer;

    public static GameController Instance { get; private set; }

    private void Awake() {
        Instance = this;
        Debug.Log("Are we starting the battle");
    }

    private void Start() 
    {
        battleSystem.Awake();
        BattleSystem.Instance.onBattleOver += EndBattle;
        
        playerController.onEnterTrainersView += (Collider2D trainerCollider) => 
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            this.trainer = trainer;
            if (trainer != null) 
            {
                state = GameState.Cutscene;    
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () =>
        {
            if(state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
    }

    

    public void StartBattle(BattleUnit trainerUnit) {
        state = GameState.Battle;
        BattleSystem.Instance.gameObject.SetActive(true);
        //SceneManager.LoadScene("BattleScene");
        worldCamera.gameObject.SetActive(false);
        BattleSystem.Instance.StartBattle(trainerUnit); 
    }

    void EndBattle(bool won) {
        if (won == true) { //true means trainer lost and false means trainer won.
            Debug.Log("trainer lost");
            trainer.BattleLost();
            //Debug.Log("it reached here");
            //trainer = null;
        }
        else {
            Debug.Log("trainer won");
            trainer.BattleWon();
        }
        state = GameState.FreeRoam;
        trainer.TrainerUnit.disableImage(); //have to disable image here or will have overlap as we are calling instance of same trainerunit scripts and when battle ends didnt account for this image
        BattleSystem.Instance.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            BattleSystem.Instance.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Cutscene)
        {
            playerController.stopCharacter();
        }
    }
}