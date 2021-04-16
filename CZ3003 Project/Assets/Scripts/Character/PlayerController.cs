// Authors: Daryl Neo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerController : MonoBehaviour
{
    public LayerMask fovLayer;
    public LayerMask NPCLayer;

    [SerializeField] BattleUnit playerUnit;

    public BattleUnit PlayerUnit 
    { 
        get { return playerUnit; }
    }

    public float speed=5f;
    public bool Moving;
    private Vector2 input;
    
    public event Action onEncountered;
    public event Action<Collider2D> onEnterTrainersView;

    private Animator animator;

    private SpriteRenderer spriteRenderer;
    // Initialize variables from Components of gameobject
    private void Awake() 
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        animator=GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        // Checks if player is within Trainer's view
        CheckIfInTrainersView();

        // Reset direction
        Vector2 dir = Vector2.zero;
        
        // Detects input and move as accordingly with the correct animation playing
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            dir.y = 0;
            animator.SetFloat("moveX",dir.x);
            animator.SetFloat("moveY",dir.y);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            dir.y = 0;
            animator.SetFloat("moveX",dir.x);
            animator.SetFloat("moveY",dir.y);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            dir.x = 0;
            animator.SetFloat("moveX",dir.x);
            animator.SetFloat("moveY",dir.y);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            dir.x = 0;
            animator.SetFloat("moveX",dir.x);
            animator.SetFloat("moveY",dir.y);
        }

        // Normalize the direction
        dir.Normalize();

        // Set animation to moving from standstill
        animator.SetBool("IsMoving", dir.magnitude > 0);

        // Applies velocity to gameobject allowing it to move in specific direction 
        GetComponent<Rigidbody2D>().velocity = speed * dir;
        

        // Interacts when player is not moving
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("IsMoving", false);
            Interact();
        }
        
    }
    
    // Interact method
    void Interact()
    {
        // Get the current direction player is facing
        var faceDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));

        // Store current position + direction
        var interactPos = transform.position+faceDir;

        var collider = Physics2D.OverlapCircle(interactPos,0.3f,NPCLayer);

        // If collider detects something
        if(collider!=null)
        {
            // Collider returns the object you are colliding with
            // Call the collided object Interact method
            collider.GetComponent<Interactable>()?.Interact(transform); 
        }
    }

    // CheckIfInTrainersView method
    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, fovLayer);
        if (collider != null)
        {
            onEnterTrainersView?.Invoke(collider);
        }
    }

    // stopCharacter method
    public void stopCharacter() {
        // Stop character's movement
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        // Set bool to false and change moving animation to standstill
        animator.SetBool("IsMoving", false);
    }
}


