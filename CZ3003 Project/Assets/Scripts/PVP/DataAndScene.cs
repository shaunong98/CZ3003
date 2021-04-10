using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DataAndScene : MonoBehaviour
{
    public LevelLoader battleLoader;
    [SerializeField] private AudioClip PVPMusic;
    public int worldNumber;
    public int sectionNumber;

    public void SceneTransition()
    {
        AudioManager.Instance.PlayMusicWithFade(PVPMusic,0.1f);
        QuestionManager.worldNumber = worldNumber;
        QuestionManager.sectionNumber = sectionNumber;
        Navigation.Instance.setAllToFalse();
        battleLoader.LoadPVP();
    }
}
