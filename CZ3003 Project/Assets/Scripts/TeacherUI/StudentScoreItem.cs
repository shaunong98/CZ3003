using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Done by Jun Hao and Zhi Fah
public class StudentScoreItem : MonoBehaviour
{
    //This is the username of the user
    public Text username;
    //This is the score that user has
    public Text Score;
    //This method pushes the username and score into the studentscoreUI item
    //_username: username of user
    //_score: score of the user
    public void CreateItem(string _username, string _score){
        username.text = _username;
        Score.text = _score;
    }
}
