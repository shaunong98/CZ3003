using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{

    public TMP_Text usernameText;
    public TMP_Text killsText;
    public TMP_Text masteryText;
    public TMP_Text xpText;

    public void NewScoreElement (string _username, int _kills, int _mastery, int _xp)
    {
        usernameText.text = _username;
        killsText.text = _kills.ToString();
        masteryText.text = _mastery.ToString();
        xpText.text = _xp.ToString();
    }

}
