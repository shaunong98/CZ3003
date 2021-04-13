// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    // Name text to be displayed on Unity battle scene
    [SerializeField] Text nameText;

    // Hp Bar to be displayed.
    [SerializeField] HPBar hpBar;

    // Bool variable to check if its a custom game.
    public bool isCustom;

    // Reference to monster class
    Monster _monster;

    // Setting battle hud data that loads from either the local memory or firebase.
    public void SetData(Monster monster, bool isPlayerUnit) {
        _monster = monster;
        if (isPlayerUnit) {
            nameText.text = FirebaseManager.username;
        }
        else if (isCustom) {
            nameText.text = CustomFirebase.createdUsername;
        }
        else{
            Debug.Log("Not custom game");
            nameText.text = monster.Base.Name;
        }
        //
        
        hpBar.SetHP((float) monster.HP/monster.MaxHp); //current / total hp
    }

    // Method that updates HP concurrently as the battle goes on.
    public IEnumerator UpdateHP() {
        yield return hpBar.SetHPSmooth((float) _monster.HP/_monster.MaxHp);
    }
}
