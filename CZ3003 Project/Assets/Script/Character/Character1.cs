using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character1 : MonoBehaviour
{
    public bool IsMoving;
    public float moveSpeed = 1f;
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public float walkTime;
    public float walkCounter;
    public Vector2 moveDir;
    //public bool isCollided;

    public NPCController npcController;

    public void Start()
    {
        npcController=GetComponent<NPCController>();
        spriteRenderer=GetComponent<SpriteRenderer>();
        animator=GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        walkCounter = walkTime;
    }

    private void Update() 
    {
        if(walkCounter>=0)
        {
            walkCounter -= Time.deltaTime;
            gameObject.transform.position = new Vector2 (transform.position.x + (moveDir.x * moveSpeed*Time.deltaTime), transform.position.y + (moveDir.y * moveSpeed*Time.deltaTime));
        }
        else
        {
            IsMoving=false;
            animator.SetBool("IsMoving", IsMoving);
            npcController.setIdle();
        }
    }
    public void LookTowards(Vector3 targetPos)
    {
        var xdiff = targetPos.x - transform.position.x;
        var ydiff = (targetPos.y - transform.position.y)+0.5f;
        Debug.Log("x "+ xdiff + " y " + (ydiff-0.5f));
        Debug.DrawLine(new Vector3(transform.position.x,transform.position.y+0.5f),targetPos,Color.blue,1.5f);
        if(xdiff>=-1||xdiff<=1||ydiff>=-1||ydiff<=1)
        {
            animator.SetFloat("moveX",Mathf.Clamp(xdiff,-1f,1f));
            animator.SetFloat("moveY",Mathf.Clamp(ydiff,-1f,1f));
        }
        else
            Debug.Log("cant see diag");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("collided with player");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("did it detect this?");
            //isCollided = true;
        }
    }


    public IEnumerator Move(Vector2 moveVec)
    {
        IsMoving=true;
        moveDir = moveVec;
        walkCounter = walkTime;
        if(walkCounter>=0)
        {   
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

        animator.SetBool("IsMoving", moveDir.magnitude > 0);
            
        yield return null;
        }
    }
}