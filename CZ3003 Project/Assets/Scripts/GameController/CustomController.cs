// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Creating CustomStates during the instantiate the end and duration of battle.
public enum CustomState{End, Battle}

public class CustomController: MonoBehaviour
{
    // PlayerController Object.
    [SerializeField] public PlayerController playerController;

    // CustomBattleSystem Object.
    [SerializeField] CustomBattleSystem battleSystem;

    // Customstate reference.
    CustomState state;

    //TrainerUnit object.
    [SerializeField] BattleUnit trainerUnit;

    // Creating an instance for custom controller to be called in another class.
    public static CustomController Instance { get; private set; }

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
        CustomBattleSystem.Instance.onBattleOver += EndBattle;
    }
    
    // Changing of scene to the custom battle scene.
    public void StartBattle(BattleUnit trainerUnit) {
        state = CustomState.Battle;
        CustomBattleSystem.Instance.gameObject.SetActive(true);
        CustomBattleSystem.Instance.StartBattle(trainerUnit);
    }

    // Method to signal the end of the battle and setting the battle scene to false.
    public void EndBattle(bool won) {
        state = CustomState.End;
        CustomBattleSystem.Instance.gameObject.SetActive(false);
    }

    // Update to be called constantly during each frames and checking to see which state Custom state is.
    private void Update()
    {
        if (state == CustomState.Battle)
        {
            CustomBattleSystem.Instance.HandleUpdate();

        }
        else if (state == CustomState.End)
        {
            Debug.Log("Did it load scene?");
            SceneManager.LoadScene("Map Selection");
        }
    }
}