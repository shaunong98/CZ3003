using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    Character character;
    public enum NPCState{ Idle, Walking, Dialog};
    public NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;


    public void Awake()
    {
        character = GetComponent<Character>();
    }
    public void Interact(Transform initiator)
    {
        if(state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(initiator.position);
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog,()=>
            {
                idleTimer=0f;
                state = NPCState.Idle;
            }));
        }
    }
    public void setIdle()
    {
        if(state==NPCState.Walking)
        state = NPCState.Idle;
    }

    public void Update()
    {
        if(state==NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > timeBetweenPattern)
            {
                idleTimer=0f;
                StartCoroutine(Walk());
            }
        }
    }

    IEnumerator Walk()
    {
        state=NPCState.Walking;
        yield return character.Move(movementPattern[currentPattern]);
        currentPattern = (currentPattern + 1) % movementPattern.Count;
    }
}
