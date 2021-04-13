using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Done by Jun Hao and Jethro
public class SingleLevel : MonoBehaviour
{
    //This is stars for the level
    private int levelStarsNum = 0;
    // This is the level index
    public int levelIndex;
    //This method is called when the star button is pressed
    //_starsNum: The star gained
    public void PressStarButton(int _starsNum)
    {
        levelStarsNum = _starsNum;

        if (levelStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))
        {
            PlayerPrefs.SetInt("Lv" + levelIndex, levelStarsNum);
        }
        Debug.Log("Saving Data is " + PlayerPrefs.GetInt("Lv" + levelIndex));

        //UIManager.instance.BackLevelSelection();
        //WorldUIManager.instance.SceneTransition("GameScene");
        //WorldUIManager.instance.gameStart();
        WorldUIManager.instance.BackLevelSelection();

    }
}
