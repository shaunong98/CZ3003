using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FirebaseFunction : MonoBehaviour
{
    [Header("UserData")]
    public TMP_Text usernameTitle;
    public TMP_Text totalStars;
    public TMP_Text totalPoints;
    public TMP_Text selectedStars;
    public TMP_Text selectedPoints;
    public Transform scoreboardContent;
    public TMP_Text ranktext;
    private void Start()
    {
        FirebaseManager.Instance.usernameTitle = usernameTitle;
        FirebaseManager.Instance.totalStars = totalStars;
        FirebaseManager.Instance.totalPoints = totalPoints;
        FirebaseManager.Instance.selectedStars = selectedStars;
        FirebaseManager.Instance.selectedPoints = selectedPoints;
        FirebaseManager.Instance.scoreboardContent = scoreboardContent;
        FirebaseManager.Instance.ranktext = ranktext;
    }
    public void ScoreboardButton()
    {
        FirebaseManager.Instance.ScoreboardButton();
    }

    public void SignOutButton()
    {
        FirebaseManager.Instance.SignOutButton();
    }

    public void DisplayStar()
    {
        FirebaseManager.Instance.displayStarsPoints();
    }

    public void ShowButton()
    {
        FirebaseManager.Instance.displayWorldSectionData();
    }

    public void ResetButton()
    {
        FirebaseManager.Instance.displayTotalPoints();
    }
}
