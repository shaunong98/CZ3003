// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Monster", menuName = "Monster/Create new Monster")]
public class MonsterBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;
    
    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] List<LearnableMoves> learnableMoves;

    public string Name {
        get{ return name; }
    }

    public string Description {
        get { return description; }
    }
    
    public Sprite FrontSprite {
        get { return frontSprite; }
    }

    public Sprite BackSprite {
        get { return backSprite; }
    }

    public int MaxHp {
        get { return maxHp; }
    }

    public int Attack {
        get { return attack; }
    }


    public List<LearnableMoves> LearnableMoves {
        get { return learnableMoves; }
    }
    
}

[System.Serializable]
public class LearnableMoves{
    [SerializeField] MoveBase moveBase;

    public MoveBase Base {
        get { return moveBase; }
    }
}
