using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Classes.Enums;

namespace Classes {
    public class NPC : Entity
    {
        public Alliance Alignment;
        public Quest[] GivableQuests;
        
        public bool IsAggressiveTowardsPlayer() { // TODO
            return true;
        }
    }
}

