using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : Entity
{
    public Alliance Alignment;
    
    public bool IsAggressiveTowardsPlayer() { // TODO
        return true;
    }
}

