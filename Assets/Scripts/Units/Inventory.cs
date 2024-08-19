using System;
using Weapons;

namespace Units {
        [Serializable]
        public struct Inventory
        {
            public Weapon item;
            public bool isUsed;
        }
}
