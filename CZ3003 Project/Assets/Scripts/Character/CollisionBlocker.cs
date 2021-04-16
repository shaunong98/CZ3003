// Authors: Daryl Neo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionBlocker : MonoBehaviour
{
    public BoxCollider2D characterCollider;
    public BoxCollider2D characterBlockerCollider;

    // Prevents player from pushing the NPCs
    void Start() 
    {
        Physics2D.IgnoreCollision(characterCollider,characterBlockerCollider,true);
    }
}
