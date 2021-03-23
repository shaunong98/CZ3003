using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelection : MonoBehaviour
{
    public bool isUnlock;
    //public GameObject lockGo;
    public GameObject unlockGo;

    public int mapIndex;
    public int questNum;
    public int startLevel;
    public int endLevel;

    private void Update()
    {
        UpdateMapStatus();
        UpdateMap();
    }
    
    private void UpdateMapStatus(){
        if(isUnlock)
        {
            unlockGo.gameObject.SetActive(true);
            //lockGo.gameObject.SetActive(false);
        }
        /*else
        {
            unlockGo.gameObject.SetActive(false);
            lockGo.gameObject.SetActive(true);
        }*/
    }
    private void UpdateMap()
    {
        if (UIManager.instance.stars > questNum)
        {
            isUnlock = true;
        }
        else
        {
            isUnlock = false;
        }
    }
}
