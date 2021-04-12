// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable
{
    // TrainerMusic variable.
    [SerializeField] private AudioClip TrainerMusic;

    // Dialog object variable.
    [SerializeField] Dialog dialog;

    // Dialog object variable after losing the battle.
    [SerializeField] Dialog dialogAfterLosingBattle;

    // Dialog Object variable after winning the battle.
    [SerializeField] Dialog dialogAfterWinningBattle;

    // Exclamation object for each trainer.
    [SerializeField] GameObject exclamation;

    // Field of vision object for each trainer.
    [SerializeField] GameObject fov;

    // BattleUnit object reference.
    [SerializeField] BattleUnit trainerUnit;

    // Unique number assigned to each trainer.
    public int trainerNumber;

    // Unique number assigned to trainer and those from the same trainers share the same section number.
    public int trainerSection;
    //[SerializeField] BattleSystem battleSystem;

    // Getter method for TrainerUnit.
    public BattleUnit TrainerUnit { 
        get { return trainerUnit; }
    }

    // Boolean variable to signal if the trainer lost (true) and if trainer has won (false)
    bool battleLost = false;

    // Boolean variable to signal if the trainer has won but is ready to battle again.
    bool battleAgain = false;

    // Reference character class.
    Character character;

    // Method called when trainercontroller class is called.
    public void Awake()
    {
        character = GetComponent<Character>();
    }

    // Method that is called when player interacts with the trainer.
    public void Interact(Transform initiator) {
        character.LookTowards(initiator.position);
        if (!battleLost) {
            if(battleAgain) {
                Debug.Log("battle won");
                StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfterWinningBattle,()=>
                {
                    StartCoroutine(GameController.Instance.StartBattle(trainerUnit));
                }));
            }
            else {
                StartCoroutine(DialogManager.Instance.ShowDialog(dialog,()=>
                {
                    StartCoroutine(GameController.Instance.StartBattle(trainerUnit));
                }));
            } 
        }
        
        else {
            Debug.Log("battle lost");
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfterLosingBattle));
        }
    }

    // Method to call the battle scene when the player meets the trainer.
    public IEnumerator TriggerTrainerBattle(PlayerController player) {
        AudioManager.Instance.PlayMusic(TrainerMusic);
        Debug.Log("exclamation set active");
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);
        Debug.Log("Start Battle");
        var diff = player.transform.position - transform.position ;
        var moveVec = diff - diff.normalized; //normalized vector keeps the same direction but its length is 1.0
        yield return character.Move(moveVec);

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog,()=>
        {
            StartCoroutine(GameController.Instance.StartBattle(trainerUnit));
        }));
    }

    // Method called when trainer lost a battle and a star is given to the player. 
    public void BattleLost() {
        Debug.Log("Battle ended");
        battleLost = true;
        string trainersection = trainerSection.ToString();
        if (PlayerPrefs.GetInt($"{trainerNumber}") == 0) {
            PlayerPrefs.SetInt($"{trainerNumber}", 1);
            //PlayerPrefs.SetInt($"Lv{trainerSection}", PlayerPrefs.GetInt($"Lv{trainerSection}") + 1);
            PlayerPrefs.SetInt("Lv" + trainersection, PlayerPrefs.GetInt("Lv" + trainersection) + 1);
        }
        fov.gameObject.SetActive(false);
        int star = PlayerPrefs.GetInt($"{trainerNumber}");
        //int totalstar = PlayerPrefs.GetInt($"Lv{trainerSection}");
        int totalstar = PlayerPrefs.GetInt("Lv" + trainersection);
        Debug.Log($"{star}");
        Debug.Log($"{totalstar}");
    }

    // Method called when a trainer wins a battle.
    public void BattleWon() {
        battleAgain = true;
        fov.gameObject.SetActive(false);
    }
}
