using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Done by Jun Hao and Jethro
public class WorldSelection : MonoBehaviour
{
    //This variable determines if the section can be unlocked
    public bool isUnlock;
    //public GameObject lockGo;
    //This is the unlocked gamee object
    public GameObject unlockGo;
    //This is the map index
    public int mapIndex;
    //This is the quest number
    public int questNum;
    //This is the start level of the world
    public int startLevel;
    //This is the end level of the world
    public int endLevel;
    //This method is called to update the script
    private void Update()
    {
        UpdateMapStatus();
        UpdateMap();
    }
    //This method unlocks the section
    private void UpdateMapStatus(){
        if(isUnlock)
        {
            unlockGo.gameObject.SetActive(true);
        }
  
    }
    //This method determines if the section is unlock
    private void UpdateMap()
    {
        if (WorldUIManager.instance.stars > questNum)
        {
            isUnlock = true;
        }
        else
        {
            isUnlock = false;
        }
    }
}
