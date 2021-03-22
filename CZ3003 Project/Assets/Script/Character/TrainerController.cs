using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;

    Character character;

    public void Awake()
    {
        character = GetComponent<Character>();
    }

    public Vector3 calcDirection(PlayerController player)
    {
        var xdiff = player.transform.position.x - transform.position.x;
        var ydiff = player.transform.position.y - transform.position.y;
        Debug.Log(xdiff);
        Debug.Log(ydiff);

        if (Mathf.Abs(xdiff)>Mathf.Abs(ydiff)) 
        {
            Debug.Log("xdiff greater");
            ydiff=0;
        }
        else if (Mathf.Abs(ydiff)>Mathf.Abs(xdiff)) 
        {
            Debug.Log("ydiff greater");
            xdiff=0;
        }  
        
        if(xdiff>0)
        {
            return new Vector3(1.3f,0,0);
        }
        else if(xdiff<0)
        {
            return new Vector3(-1.3f,0,0);
        }
        else if(ydiff>0)
        {
            return new Vector3(0,1.3f,0);
        }
        else if(ydiff<0)
        {
            return new Vector3(0,-1.3f,0);
        }
        return new Vector3(0,0,0);
    }
    public IEnumerator TriggerTrainerBattle(PlayerController player) {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        var diff = player.transform.position - transform.position + calcDirection(player);
        Debug.Log(diff);
        var moveVec = diff - diff.normalized; //normalized vector keeps the same direction but its length is 1.0
        yield return character.Move(diff);

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog,()=>
            {
               Debug.Log("start trainer battle");
            }));
    }
}
