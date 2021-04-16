
// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
  // Reference to MonsterBase with getter, setter method.
  public MonsterBase Base { get; set; }

  // Health Points.
  public int HP { get; set; }

  // List of Moves its able to use. 
  public List <Move> Moves { get; set; }

  // Monster Constructor.
  public Monster(MonsterBase pBase) {
      Base = pBase;
      HP = MaxHp;

      //Generate Moves base on level
      Moves = new List<Move>(); //Move is a class that contains moves which will be assigned to each monster
      foreach (var move in Base.LearnableMoves) { //loop through the learnable moves of a pokemon and check if it matches level
          Moves.Add(new Move(move.Base));
      }
  }

  // Max Health Points.
  public int MaxHp {
      get { return Mathf.FloorToInt(Base.MaxHp); }
  }

  // Method that causes player/trainer to take damage. 
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

  // Method that gets a random move for the trainer.
  public Move GetRandomMove() {
    int r = Random.Range(0, Moves.Count);
    return Moves[r];
  }
}
