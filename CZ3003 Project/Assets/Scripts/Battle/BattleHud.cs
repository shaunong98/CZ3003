using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    public bool isCustom;

    Monster _monster;

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
        
        levelText.text = "Lvl " + monster.Level;
        hpBar.SetHP((float) monster.HP/monster.MaxHp); //current / total hp
    }

    public IEnumerator UpdateHP() {
        yield return hpBar.SetHPSmooth((float) _monster.HP/_monster.MaxHp);
    }
}
