using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* * There isn't a class called Monster or anything like that
* We can consider something a monster
* maybe if it wants to attack us, which we can detect using IsAggresiveTowardsPlayer() method
* which is based on the reputation of the player.
*
* * If a monster has special traits, create new class inheriting NPC
*/
public class NPC : Entity
{
    public Alliance Alignment;
    
    public bool IsAggressiveTowardsPlayer() { // TODO
        return true;
    }
}

