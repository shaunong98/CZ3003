// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Creating PVPStates during the instantiate the end and duration of battle.
public enum PVPState{End, Battle}

public class PVPController: MonoBehaviour
{
    // PlayerController Object.
    [SerializeField] public PlayerController playerController;

    //BattleSystem Object.
    [SerializeField] BattleSystem battleSystem;

    // PVPstate reference.
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
    }

    

    public void StartBattle(BattleUnit trainerUnit) {
        state = PVPState.Battle;
        BattleSystem.Instance.gameObject.SetActive(true);
        BattleSystem.Instance.StartBattle(trainerUnit); 
    }

    public void EndBattle(bool won) {
        state = PVPState.End;
        BattleSystem.Instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == PVPState.Battle)
        {
            BattleSystem.Instance.HandleUpdate();

        }
        else if (state == PVPState.End)
        {
            Debug.Log("Did it load scene?");
            Navigation.Instance.backToWorldPVPScreen(QuestionManager.worldNumber - 1);
        }
    }
}