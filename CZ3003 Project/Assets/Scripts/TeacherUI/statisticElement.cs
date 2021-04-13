using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Done by Jun Hao and Zhi Fah
public class StatisticElement : MonoBehaviour
{
    //This is the username of the user
    public Text username;
    //This is the Stars that the user obtained
    public Text Stars;
    //This is the points the user has obtained
    public Text Points;
    //This method will pass the username, stars and points into the Statistic Element
    //_username: username for the user
    //star: stars obtained by the user
    //points: points obtained by the user
    public void NewScoreElement(string _username, int stars, int points)
    {
        username.text = _username;
        Stars.text = stars.ToString();
        Points.text = points.ToString();
    }
}
