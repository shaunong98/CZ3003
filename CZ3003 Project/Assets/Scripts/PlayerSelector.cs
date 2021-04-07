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

    public GameObject battleSystem;

    public bool isPVP;

    // Start is called before the first frame update
    void Awake()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");
        // 0 - calem, 1 - brendan, 2 - may, 3 - serena
        if(index==0)
        {
            if (isPVP) {
                gameManager.GetComponent<PVPController>().playerController=calemPlayer.GetComponent<PlayerController>();
            }
            else {
                cam.GetComponent<CameraFollow>().target= calemPlayer.transform;
                gameManager.GetComponent<GameController>().playerController=calemPlayer.GetComponent<PlayerController>();
            }
            calemPlayer.gameObject.SetActive(true);
            //gameManager.GetComponent<GameController>().playerController=calemPlayer.GetComponent<PlayerController>();
            battleSystem.GetComponent<BattleSystem>().player=calemPlayer.GetComponent<PlayerController>();
            //cam.GetComponent<CameraFollow>().target= calemPlayer.transform;
        }

        else if(index==1)
        {
            if (isPVP) {
                gameManager.GetComponent<PVPController>().playerController=brendanPlayer.GetComponent<PlayerController>();
            }
            else {
                gameManager.GetComponent<GameController>().playerController=brendanPlayer.GetComponent<PlayerController>();
                cam.GetComponent<CameraFollow>().target= brendanPlayer.transform;
            }
            brendanPlayer.gameObject.SetActive(true);
            battleSystem.GetComponent<BattleSystem>().player=brendanPlayer.GetComponent<PlayerController>();
            
            
        }

        else if(index==2)
        {
            if (isPVP) {
                gameManager.GetComponent<PVPController>().playerController=mayPlayer.GetComponent<PlayerController>();
            }
            else {
                gameManager.GetComponent<GameController>().playerController=mayPlayer.GetComponent<PlayerController>();
                cam.GetComponent<CameraFollow>().target= mayPlayer.transform;
            }
            mayPlayer.gameObject.SetActive(true);
            battleSystem.GetComponent<BattleSystem>().player=mayPlayer.GetComponent<PlayerController>();
            
        }

        else if(index==3)
        {
            if (isPVP) {
                gameManager.GetComponent<PVPController>().playerController=serenaPlayer.GetComponent<PlayerController>();
            }
            else {
                gameManager.GetComponent<GameController>().playerController=serenaPlayer.GetComponent<PlayerController>();
                cam.GetComponent<CameraFollow>().target= serenaPlayer.transform;
            }
            serenaPlayer.gameObject.SetActive(true);
            battleSystem.GetComponent<BattleSystem>().player=serenaPlayer.GetComponent<PlayerController>();
        }
    }
}
