using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Entity class provides typical properties for alive creatures like HP or Attack, but it
* can obviously be used as well for the undead.
*/
public class Entity : InteractableObject
{
    public float MaxHP;
    public float Attack;
    public float Defense;
    public Object Inventory; // TODO unless we decide to not implement any kind of inventory
    

    protected float _currentHP;

    protected virtual void Awake() {
        _currentHP = MaxHP;
    }
}
