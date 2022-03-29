using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Every single object in the game that has some kind of interaction like for example gatherable stones,
* NPCs, the Player himself or sound sources should be considered as InteractableObject.
*/
public class InteractableObject : MonoBehaviour
{
    public string Name;

    /**
    * * Important! ID is not an identifier of a unique instance. There can be 2 objects having the same ID.
    * * The reason is to throw everything of the same kind into the same bag; imagine 2 goblins that have
    * * the same ID, which purpose is to make it easier for for example kill quests.
    */
    public int ID;
}
