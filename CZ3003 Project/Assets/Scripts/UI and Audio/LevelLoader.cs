// Authors: Daryl Neo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime=1f;

    // Starts a coroutine for scene swap to "Character Selection" Scene
    public void LoadCharSel()
    {
        StartCoroutine(LoadLevel("Character Selection"));
    }
        
    // Starts a coroutine for scene swap to "Map Selection" Scene
    public void LoadMain()
    {
        StartCoroutine(LoadLevel("Map Selection"));
    }

    // Starts a coroutine for scene swap to "BattleScene" Scene
    public void LoadPVP()
    {
        StartCoroutine(LoadLevel("BattleScene"));
    }

    // Starts a coroutine for scene swap to "CustomBattleScene" Scene
    public void LoadCustom()
    {
        StartCoroutine(LoadLevel("CustomBattleScene"));
    }

    // Starts a coroutine for scene swap to specific Level in parameter
    public void Loadlevel(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    // Triggers the animation fade to black to transition to the next scene
    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }

    // Triggers the animation fade to black
    public void FadetoBlack()
    {
        transition.SetTrigger("Start");
    }

    // Triggers the unfade animation
    public void UnFade()
    {
        transition.SetTrigger("End");
    }
}
