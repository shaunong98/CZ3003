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

    // TrainerUnit object.
    [SerializeField] BattleUnit trainerUnit;

    // Creating an instance for PVP controller to be called in another class.
    public static PVPController Instance { get; private set; }

    // Awake method to be called when this class is instantiated.
    private void Awake() {
        Instance = this;
        Debug.Log("Are we starting the battle");
    }

    // Start is called before the first frame update
    private void Start() 
    {
        battleSystem.Awake();
        StartBattle(trainerUnit); 
        BattleSystem.Instance.onBattleOver += EndBattle;
    }

    // Changing of scene to the PVP battle scene.
    public void StartBattle(BattleUnit trainerUnit) {
        state = PVPState.Battle;
        BattleSystem.Instance.gameObject.SetActive(true);
        BattleSystem.Instance.StartBattle(trainerUnit); 
    }

    // Method to signal the end of the battle and setting the battle scene to false.
    public void EndBattle(bool won) {
        state = PVPState.End;
        BattleSystem.Instance.gameObject.SetActive(false);
    }

    // // Update to be called constantly during each frames and checking to see which state Custom state is.
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