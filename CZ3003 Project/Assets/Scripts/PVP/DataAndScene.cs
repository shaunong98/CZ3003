using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataAndScene : MonoBehaviour
{
    public int worldNumber;
    public int sectionNumber;

    public void SceneTransition()
    {
        QuestionManager.worldNumber = worldNumber;
        QuestionManager.sectionNumber = sectionNumber;
        Navigation.Instance.setAllToFalse();
        SceneManager.LoadScene("BattleScene");
    }
}
