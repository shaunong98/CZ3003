// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Monster", menuName = "Monster/Create new Monster")]
public class MonsterBase : ScriptableObject
{   
    // Name of player/trainer.
    [SerializeField] string name;

    // Front Sprite of player/trainer.
    [SerializeField] Sprite frontSprite;

    // Back Sprite of player/trainer.
    [SerializeField] Sprite backSprite;
    
    //Base Stats
    // Base MaxHp.
    [SerializeField] int maxHp;

    // Base Attack power.
    [SerializeField] int attack;

    // Learnable moves.
    [SerializeField] List<LearnableMoves> learnableMoves;

    // Getter Method for name.
    public string Name {
        get{ return name; }
    }
    
    // Getter Method for Front Sprite.
    public Sprite FrontSprite {
        get { return frontSprite; }
    }

    // Getter method for Back Sprite.
    public Sprite BackSprite {
        get { return backSprite; }
    }

    // Getter Method for base max HP.
    public int MaxHp {
        get { return maxHp; }
    }

    // Getter method for attack.
    public int Attack {
        get { return attack; }
    }

    // Getter moves for the list of learnable moves.
    public List<LearnableMoves> LearnableMoves {
        get { return learnableMoves; }
    }
    
}

[System.Serializable]
// Learnable moves class that contains the base move.
public class LearnableMoves{
    [SerializeField] MoveBase moveBase;

    // Getter method for base moves.
    public MoveBase Base {
        get { return moveBase; }
    }
}
