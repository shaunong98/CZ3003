using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
  public MonsterBase Base { get; set; }

  public int Level{ get; set; }

  public int HP { get; set; }

  public List <Move> Moves { get; set; }



  public Monster(MonsterBase pBase, int plevel) {
      Base = pBase;
      Level = plevel;
      HP = MaxHp;

      //Generate Moves base on level
      Moves = new List<Move>(); //Move is a class that contains moves which will be assigned to each monster
      foreach (var move in Base.LearnableMoves) { //loop through the learnable moves of a pokemon and check if it matches level
        if (move.Level <= Level) {
            Moves.Add(new Move(move.Base));
        }

        //if (Moves.Count >= 4) break;
        
      }
  }

  public int Attack {
      get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
  }

  //might change
  public int MaxHp {
      get { return Mathf.FloorToInt(Base.MaxHp); }
  }
  
  public bool TakeDamage(Move move) {
    //Debug.Log($"{HP}");
    int damage = move.Base.Power;
    HP -= damage;
    if (HP <= 0) {
      HP = 0;
      return true;
    }
    return false;
  }

  public bool TakeDamageWrong(Move move) {
    //Debug.Log($"{HP}");
    int damage =  move.Base.Power*2;
    HP -= damage;
    Debug.Log($"{damage}");
    if (HP <= 0) {
      HP = 0;
      return true;
    }
    return false;
  }

  public Move GetRandomMove() {
    int r = Random.Range(0, Moves.Count);
    return Moves[r];
  }
}
