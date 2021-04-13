// Authors: Daryl Neo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public bool IsMoving;
    public float moveSpeed = 1f;
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public float walkTime;
    public float walkCounter;
    public Vector2 moveDir;
    public bool isCollided;
    public NPCController npcController;

    // Initialize Values and Components
    public void Start()
    {
        npcController = GetComponent<NPCController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        walkCounter = walkTime;
    }

    private void Update() 
    {
        // Counts down walkCounter duration
        if(walkCounter>=0)
        {
            walkCounter -= Time.deltaTime;
        }
        // Stop moving when counter reaches 0
        else
        {
            // Stops movement
            myRigidbody.velocity = Vector2.zero;

            // Set bool to false
            IsMoving=false;

            // Disable Moving animation to stand still
            animator.SetBool("IsMoving", false);

            // Update current state of NPC
            npcController.setIdle();
        }
    }

    // Method called to make NPCs face player when interacting
    public void LookTowards(Vector3 targetPos)
    {
        // Calc position differences with Y offset
        var xdiff = targetPos.x - transform.position.x;
        var ydiff = (targetPos.y - transform.position.y)+0.5f;

        // If X Y position differences falls within range
        if(xdiff>=-1||xdiff<=1||ydiff>=-1||ydiff<=1)
        {
            // Face correct direction of the player interacting
            animator.SetFloat("moveX",Mathf.Clamp(xdiff,-1f,1f));
            animator.SetFloat("moveY",Mathf.Clamp(ydiff,-1f,1f));
        }
    }

    // Movement script for Characters
    public IEnumerator Move(Vector2 moveVec)
    {
        // Set bool to true
        IsMoving=true;
        
        moveDir = moveVec;

        // Reset walkCounter to predefined values
        walkCounter = walkTime;

        // Allowed to walk if walkCounter is >=0
        if(walkCounter>=0)
        {   
            // Set valid directions for animations and sprites
            if(moveVec.x>0)
            {
                animator.SetFloat("moveX",moveVec.x);
                animator.SetFloat("moveY",moveVec.y);
            }
            else if(moveVec.x<0)
            {
                animator.SetFloat("moveX",moveVec.x);
                animator.SetFloat("moveY",moveVec.y);
            }
            else if(moveVec.y>0)
            {
                animator.SetFloat("moveX",moveVec.x);
                animator.SetFloat("moveY",moveVec.y);
            }
            else if(moveVec.y<0)
            {   
                animator.SetFloat("moveX",moveVec.x);
                animator.SetFloat("moveY",moveVec.y);
            }
            // Set bool to true
            IsMoving=true;

        // Set animation to walking animation from standstill
        animator.SetBool("IsMoving", moveDir.magnitude > 0);
        yield return null;
       
        }
        // Apply velocity in appropriate direction
        myRigidbody.velocity = new Vector2 (moveDir.x * moveSpeed,moveDir.y * moveSpeed);
    }
    
}