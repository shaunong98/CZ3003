using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public GameObject[] World;
    public GameObject SelectionPanel;

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
}
