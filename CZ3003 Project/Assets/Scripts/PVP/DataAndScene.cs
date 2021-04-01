using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataAndScene : MonoBehaviour
{
    public int WorldNumber;
    public int SectionNumber;

    public void SceneTransition(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
