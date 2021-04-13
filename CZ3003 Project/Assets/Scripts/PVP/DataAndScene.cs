using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Done by Jethro
public class DataAndScene : MonoBehaviour
{
    //This is the instance of level loader
    public LevelLoader battleLoader;
    //This is the music for the gameplay
    [SerializeField] private AudioClip PVPMusic;
    //This is the world which is selected
    public int worldNumber;
    //This is the section selected
    public int sectionNumber;
    //This method is called to enter the gameplay for PVP
    public void SceneTransition()
    {
        AudioManager.Instance.PlayMusicWithFade(PVPMusic,0.1f);
        QuestionManager.worldNumber = worldNumber;
        QuestionManager.sectionNumber = sectionNumber;
        Navigation.Instance.setAllToFalse();
        battleLoader.LoadPVP();
    }
}
