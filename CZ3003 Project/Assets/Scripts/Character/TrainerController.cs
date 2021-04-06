using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfterLosingBattle;
    [SerializeField] Dialog dialogAfterWinningBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;
    [SerializeField] BattleUnit trainerUnit;
    //[SerializeField] BattleSystem battleSystem;

    public BattleUnit TrainerUnit { 
        get { return trainerUnit; }
    }

    bool battleLost = false;
    bool battleAgain = false;
    Character character;

    public void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initiator) {
        character.LookTowards(initiator.position);
        if (!battleLost) {
            if(battleAgain) {
                Debug.Log("battle won");
                StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfterWinningBattle,()=>
                {
                    GameController.Instance.StartBattle(trainerUnit);
                }));
            }
            else {
                StartCoroutine(DialogManager.Instance.ShowDialog(dialog,()=>
                {
                    GameController.Instance.StartBattle(trainerUnit);
                }));
            } 
        }
        
        else {
            Debug.Log("battle lost");
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfterLosingBattle));
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player) {
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
                GameController.Instance.StartBattle(trainerUnit);
            }));
    }

    public void BattleLost() {
        Debug.Log("Battle ended");
        battleLost = true;
        fov.gameObject.SetActive(false);
    }
    public void BattleWon() {
        battleAgain = true;
        fov.gameObject.SetActive(false);
    }

}
