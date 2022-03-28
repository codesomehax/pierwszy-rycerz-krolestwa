using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Classes {
    public class Entity : MonoBehaviour
    {
        public string EntityName;
        public float MaxHP;
        public float Attack;
        public float Defense;

        

        protected float _currentHP;
    }
}

