using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    private int index;
    public GameObject brendanPlayer;
    public GameObject calemPlayer;
    public GameObject mayPlayer;
    public GameObject serenaPlayer;
    public GameObject cam;

    public GameObject gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");
        // 0 - calem, 1 - brendan, 2 - may, 3 - serena
        if(index==0)
        {
            calemPlayer.gameObject.SetActive(true);
            gameManager.GetComponent<GameController>().playerController=calemPlayer.GetComponent<PlayerController>();
            cam.GetComponent<CameraFollow>().target= calemPlayer.transform;
        }

        else if(index==1)
        {
            brendanPlayer.gameObject.SetActive(true);
            gameManager.GetComponent<GameController>().playerController=brendanPlayer.GetComponent<PlayerController>();
        }

        else if(index==2)
        {
            mayPlayer.gameObject.SetActive(true);
            gameManager.GetComponent<GameController>().playerController=mayPlayer.GetComponent<PlayerController>();
        }

        else if(index==3)
        {
            serenaPlayer.gameObject.SetActive(true);
            gameManager.GetComponent<GameController>().playerController=serenaPlayer.GetComponent<PlayerController>();
        }
    }
}
