// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move class to contain the base moves for each player/trainer.
public class Move
{
    // Getter/setter method for base moves.
   public MoveBase Base { get; set; }

    // Constructor method for move.
   public Move(MoveBase pBase) {
       Base = pBase;
   }
}
