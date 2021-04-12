using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime=1f;
    public void LoadCharSel()
    {
        StartCoroutine(LoadLevel("Character Selection"));
    }
    public void LoadMain()
    {
        StartCoroutine(LoadLevel("Map Selection"));
    }

    public void LoadPVP()
    {
        StartCoroutine(LoadLevel("BattleScene"));
    }

    public void LoadCustom()
    {
        StartCoroutine(LoadLevel("CustomBattleScene"));
    }

    public void Loadlevel(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }

    public void FadetoBlack()
    {
        transition.SetTrigger("Start");
    }

    public void UnFade()
    {
        transition.SetTrigger("End");
    }
}
