using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class statisticElement : MonoBehaviour
{
    public TMP_Text usernameText;
    //public TMP_Text killsText;
    public TMP_Text masteryText;
    public TMP_Text xpText;

    public void NewScoreElement(string _username, int stars, int points)
    {
        usernameText.text = _username;
        masteryText.text = stars.ToString();
        xpText.text = points.ToString();
    }
}
