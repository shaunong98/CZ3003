using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{

    public TMP_Text usernameText;
    public TMP_Text worldText;
    public TMP_Text sectionText;
    public TMP_Text pointsText;

    public void NewScoreElement (string _username, string _world, string _section, int _points)
    {
        usernameText.text = _username;
        worldText.text = _world;
        sectionText.text = _section;
        pointsText.text = _points.ToString();
    }

}
