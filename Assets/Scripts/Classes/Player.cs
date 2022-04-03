using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Object supposed to hold player related properties and methods.
*/
public class Player : Entity
{
    private Dictionary<Alliance, float> _reputation;

    public float GetReputation(Alliance alliance) {
        return _reputation[alliance];
    }
}

