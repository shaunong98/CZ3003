using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CustomState{End, Battle}

public class CustomController: MonoBehaviour
{
    [SerializeField] public PlayerController playerController;
    [SerializeField] CustomBattleSystem battleSystem;
    //[SerializeField] Camera worldCamera;

    PVPState state;

    [SerializeField] BattleUnit trainerUnit;

    public static CustomController Instance { get; private set; }

    private void Awake() {
        Instance = this;
        Debug.Log("Are we starting the battle");
    }

    private void Start() 
    {
        battleSystem.Awake();
        StartBattle(trainerUnit); 
        CustomBattleSystem.Instance.onBattleOver += EndBattle;
        
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
        CustomBattleSystem.Instance.gameObject.SetActive(true);
        CustomBattleSystem.Instance.StartBattle(trainerUnit);
    }

    public void EndBattle(bool won) {
        state = PVPState.End;
        CustomBattleSystem.Instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == PVPState.Battle)
        {
            CustomBattleSystem.Instance.HandleUpdate();

        }
        else if (state == PVPState.End)
        {
            Debug.Log("Did it load scene?");
            SceneManager.LoadScene("Map Selection");
            //Navigation.Instance.backToWorldPVPScreen(QuestionManager.worldNumber - 1);
        }
    }
}