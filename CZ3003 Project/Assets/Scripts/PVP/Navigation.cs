using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Done by Jethro

public class Navigation : MonoBehaviour
{
    //This is an array of the worlds
    public GameObject[] World;
    //This is the panel where the user can select the worlds
    public GameObject SelectionPanel;
    //This is an instance of the navigation object
    public static Navigation Instance;
    //This set the current instance to navigation
    void Awake() {
        Instance = this;
    }
    //This method is called when the user clicks on the world that he wants to play
    //_index: the index of the world the user wants to play
    public void ClickWorld(int _index)
    {
        World[_index].gameObject.SetActive(true);
        SelectionPanel.gameObject.SetActive(false);
    }
    //This method is called when the user clicks the back button and returnms back to world selection
    public void Backbutton()
    {
        for (int i=0; i<World.Length;i++)
        {
            World[i].gameObject.SetActive(false);
        }
        SelectionPanel.gameObject.SetActive(true);
    }
    //This method clears the screen of any panels
    public void setAllToFalse() {
        for (int i=0; i<World.Length;i++)
        {
            World[i].gameObject.SetActive(false);
        }
        SelectionPanel.gameObject.SetActive(false);
    }
    //This methods allows the user to return from playing the PVP mode
    //_index: the index of the world the user has played
    public void backToWorldPVPScreen(int index) {
        SceneManager.LoadScene("Map Selection");
    }
}
