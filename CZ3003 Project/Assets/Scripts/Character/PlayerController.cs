using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerController : MonoBehaviour
{
    public LayerMask fovLayer;
    public LayerMask NPCLayer;

    [SerializeField] BattleUnit playerUnit;

    public BattleUnit PlayerUnit { 
        get { return playerUnit; }
    }

    public float speed=5f;
    public bool Moving;
    private Vector2 input;
    
    public event Action onEncountered;
    public event Action<Collider2D> onEnterTrainersView;

    private Animator animator;

    private SpriteRenderer spriteRenderer;
    private void Awake() 
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        animator=GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        CheckIfInTrainersView();
        Vector2 dir = Vector2.zero;
        
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

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);
        GetComponent<Rigidbody2D>().velocity = speed * dir;
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("IsMoving", false);
            Interact();
        }
        
    }
    
    void Interact()
    {
        var faceDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position+faceDir;
       
        var collider = Physics2D.OverlapCircle(interactPos,0.3f,NPCLayer);
        if(collider!=null)
        {
            //onEncountered();
            collider.GetComponent<Interactable>()?.Interact(transform); //collider returns the object you are colliding with
        }
    }

    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, fovLayer);
        if (collider != null)
        {
            onEnterTrainersView?.Invoke(collider);
        }
    }

    public void stopCharacter() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }
}


