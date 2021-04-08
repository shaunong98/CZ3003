using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{FreeRoam, Battle, Dialog, Cutscene}

public class GameController : MonoBehaviour
{
    public LevelLoader battleLoader;
    [SerializeField] public PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    [SerializeField] private AudioClip BattleMusic;
    [SerializeField] private AudioClip VictoryMusic;

    [SerializeField] private AudioClip LevelMusic;


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

    

    public IEnumerator StartBattle(BattleUnit trainerUnit) {
        state = GameState.Battle;
        AudioManager.Instance.PlayMusic(BattleMusic);
        battleLoader.FadetoBlack();
        yield return new WaitForSeconds(1f);
        BattleSystem.Instance.gameObject.SetActive(true);
        battleLoader.UnFade();
        //SceneManager.LoadScene("BattleScene");
        worldCamera.gameObject.SetActive(false);
        BattleSystem.Instance.StartBattle(trainerUnit); 
    }

    void EndBattle(bool won) {
        if (won == true) { //true means trainer lost and false means trainer won.
            AudioManager.Instance.PlayMusicWithFade(VictoryMusic,0.1f);
            Debug.Log("trainer lost");
            trainer.BattleLost();
            //Debug.Log("it reached here");
            //trainer = null;
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