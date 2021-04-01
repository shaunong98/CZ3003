using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PVPState{End, Battle}

public class PVPController: MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    //[SerializeField] CountdownTimer countDown;
    //[SerializeField] Camera worldCamera;

    PVPState state;

    [SerializeField] BattleUnit trainerUnit;

    public static PVPController Instance { get; private set; }

    private void Awake() {
        Instance = this;
        Debug.Log("Are we starting the battle");
    }

    private void Start() 
    {
        battleSystem.Awake();
        StartBattle(trainerUnit); 
        BattleSystem.Instance.onBattleOver += EndBattle;
        
        // playerController.onEnterTrainersView += (Collider2D trainerCollider) => 
        // {
        //     var trainer = trainerCollider.GetComponentInParent<TrainerController>();
        //     this.trainer = trainer;
        //     if (trainer != null) {
        //         state = GameState.Cutscene;
                
        //         StartCoroutine(trainer.TriggerTrainerBattle(playerController));
        //     }
        // };

        // DialogManager.Instance.OnShowDialog += () =>
        // {
        //     state = GameState.Dialog;
        // };

        // DialogManager.Instance.OnCloseDialog += () =>
        // {
        //     if(state == GameState.Dialog)
        //         state = GameState.FreeRoam;
        // };
    }

    

    public void StartBattle(BattleUnit trainerUnit) {
        state = PVPState.Battle;
        BattleSystem.Instance.gameObject.SetActive(true);
        BattleSystem.Instance.StartBattle(trainerUnit); 
    }

    void EndBattle(bool won) {
        state = PVPState.End;
        BattleSystem.Instance.gameObject.SetActive(false);
        //SceneManager.LoadScene("Map Selection");
        //worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == PVPState.Battle)
        {
            BattleSystem.Instance.HandleUpdate();
            //countDown.TimerUpdate();

        }
        else if (state == PVPState.End)
        {
            SceneManager.LoadScene("Map Selection");
        }
    }
}