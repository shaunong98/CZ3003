// Authors: Jethro
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Move", menuName = "Monster/Create new Move")]
public class MoveBase : ScriptableObject
{
    // Name of move.
    [SerializeField] string name;

    [TextArea]

    // Description of move.
    [SerializeField] string description;

    // Move power damage.
    [SerializeField] int power;

    // Getter method for name.
    public string Name {
        get{ return name; }
    }

    // Getter method for description.
    public string Description {
        get { return description; }
    }

    // Getter method for power.
    public int Power {
        get { return power; }
    }
}
