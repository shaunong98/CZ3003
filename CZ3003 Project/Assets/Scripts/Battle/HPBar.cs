// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    // Health bar variable.
    [SerializeField] GameObject health; //this allow us to create a field for us to assign the gameobject

    // Set the health bar at the beginning.
    public void SetHP(float hpNormalized) {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    // Method that reduces the health at a steady pace.
    public IEnumerator SetHPSmooth(float newHp) {
        float curHp = health.transform.localScale.x;
        float changeAmt = curHp - newHp;

        while (curHp - newHp > Mathf.Epsilon) {
            curHp -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(curHp, 1f);
            yield return null;
        }
         health.transform.localScale = new Vector3(newHp, 1f);
    }
}
