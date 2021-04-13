// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState{FreeRoam, Battle, Dialog, Cutscene}

public class GameController : MonoBehaviour
{
    // Reference to levelloader that gives the black screen after every scene transition.
    public LevelLoader battleLoader;

    // PlayerController Object.
    [SerializeField] public PlayerController playerController;

    // CustomBattleSystem Object.
    [SerializeField] BattleSystem battleSystem;

    // Camera Object
    [SerializeField] Camera worldCamera;

    [SerializeField] private AudioClip BattleMusic;
    [SerializeField] private AudioClip VictoryMusic;

    [SerializeField] private AudioClip LevelMusic;

    // Gamestate reference.
    GameState state;

    // Trainer controller reference
    TrainerController trainer;

    // Creating an instance for game controller to be called in another class.
    public static GameController Instance { get; private set; }

    // Awake method to be called when this class is instantiated.
    private void Awake() {
        Instance = this;
        Debug.Log("Are we starting the battle");
    }

    // Start is called before the first frame update. If the player enters the trainer's view, the battle will be triggered.
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

    
    // Method that changes camera view from the map to the battlescene.
    public IEnumerator StartBattle(BattleUnit trainerUnit) {
        state = GameState.Battle;
        AudioManager.Instance.PlayMusic(BattleMusic);
        battleLoader.FadetoBlack();
        yield return new WaitForSeconds(1f);
        BattleSystem.Instance.gameObject.SetActive(true);
        battleLoader.UnFade();
        worldCamera.gameObject.SetActive(false);
        BattleSystem.Instance.StartBattle(trainerUnit); 
    }

    // Method to signal the end of the battle and setting the battle scene to false.
    void EndBattle(bool won) {
        if (won == true) { //true means trainer lost and false means trainer won.
            AudioManager.Instance.PlayMusicWithFade(VictoryMusic,0.1f);
            Debug.Log("trainer lost");
            trainer.BattleLost();
            battleLoader.FadetoBlack();
        }
        else {
            Debug.Log("trainer won");
            trainer.BattleWon();
            battleLoader.FadetoBlack();
        }
        state = GameState.FreeRoam;
        AudioManager.Instance.PlayMusicWithFade(LevelMusic,0.1f);
        battleLoader.UnFade();
        trainer.TrainerUnit.disableImage(); //have to disable image here or will have overlap as we are calling instance of same trainerunit scripts and when battle ends didnt account for this image
        BattleSystem.Instance.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    // Update to be called constantly during each frames and checking to see which state Game state is.
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