using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleQuestions : MonoBehaviour
{
    [SerializeField] Question questions;
    //[SerializeField] Text questionText;

    public Question Questions {
        get { return questions; }
    }
    //[SerializeField] bool isPlayerUnit;
    

    // public void SetQuestions() {
    //     Monster = new Monster(_base, level);
    //     if (isPlayerUnit) {
    //         GetComponent<Image>().sprite = Monster.Base.BackSprite;
    //     }
    //     else
    //         GetComponent<Image>().sprite = Monster.Base.FrontSprite;
    // }
}
