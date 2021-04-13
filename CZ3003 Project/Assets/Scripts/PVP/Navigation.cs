using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public GameObject[] World;
    public GameObject SelectionPanel;
    public static Navigation Instance;

    void Awake() {
        Instance = this;
    }

    public void ClickWorld(int _index)
    {
        World[_index].gameObject.SetActive(true);
        SelectionPanel.gameObject.SetActive(false);
    }
    public void Backbutton()
    {
        for (int i=0; i<World.Length;i++)
        {
            World[i].gameObject.SetActive(false);
        }
        SelectionPanel.gameObject.SetActive(true);
    }

    public void setAllToFalse() {
        for (int i=0; i<World.Length;i++)
        {
            World[i].gameObject.SetActive(false);
        }
        SelectionPanel.gameObject.SetActive(false);
    }

    public void backToWorldPVPScreen(int index) {
        SceneManager.LoadScene("Map Selection");
    }
}
