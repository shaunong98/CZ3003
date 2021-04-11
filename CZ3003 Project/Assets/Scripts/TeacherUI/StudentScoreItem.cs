using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentScoreItem : MonoBehaviour
{
    public Text username;
    public Text Score;
    public void CreateItem(string _username, string _score){
        username.text = _username;
        Score.text = _score;
    }
}
