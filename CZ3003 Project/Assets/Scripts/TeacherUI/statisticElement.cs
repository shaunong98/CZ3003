using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Done by Jun Hao and Zhi Fah
public class StatisticElement : MonoBehaviour
{
    public Text username;
    public Text Stars;
    public Text Points;

    public void NewScoreElement(string _username, int stars, int points)
    {
        username.text = _username;
        Stars.text = stars.ToString();
        Points.text = points.ToString();
    }
}
