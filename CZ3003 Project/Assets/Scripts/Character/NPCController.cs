// Authors: Daryl Neo, Jethro Phuah
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

    // Get Character component from gameobject
    public void Awake()
    {
        character = GetComponent<Character>();
    }

    // Interacting with the NPCs
    public void Interact(Transform initiator)
    {
        // If NPC current state is Idle
        if(state == NPCState.Idle)
        {
            // Change state to Dialog
            state = NPCState.Dialog;

            // Make NPCs face the player
            character.LookTowards(initiator.position);

            // Start coroutine of Dialog with NPC
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog,()=>
            {
                // Reset idleTimer
                idleTimer=0f;
                
                // Reset State back to Idle after interaction
                state = NPCState.Idle;
            }));
        }
    }
    
    // Set NPC state to idle
    public void setIdle()
    {
        // If NPC is in Walking state, change it to Idle state
        if(state==NPCState.Walking)
        state = NPCState.Idle;
    }

    public void Update()
    {
        // If NPC is in Idle state
        if(state==NPCState.Idle)
        {
            // Increase Idle Timer count
            idleTimer += Time.deltaTime;

            // If idleTimer has reached timeBetweenPattern
            if(idleTimer > timeBetweenPattern)
            {
                // Reset idleTimer
                idleTimer=0f;

                // Start Walk Coroutine
                StartCoroutine(Walk());
            }
        }
    }

    // Coroutine for Walk
    IEnumerator Walk()
    {
        // Set NPC to walking state
        state=NPCState.Walking;

        // Move the character depending on current pattern
        yield return character.Move(movementPattern[currentPattern]);
        // Change pattern to next pattern
        currentPattern = (currentPattern + 1) % movementPattern.Count;
    }
}
