using System;
using UnityEngine;
using Units;

namespace GamePlay {
    public class Army : MonoBehaviour
    {
        public event Action OnArmyWasMoved; // Broadcastable event

        public Unit[] army; // Units

        void Update()
        {
            bool armyWasMove = VerifyArmyWasMove();
            if(armyWasMove)
            {
                OnArmyWasMoved?.Invoke();
                ResetArmyMoveState();
            }
        }

        bool VerifyArmyWasMove()
        {
            foreach (var unit in army)
            {
                if(unit.IsWasMoved == false) return false;
            }
            return true;
        }

        void ResetArmyMoveState()
        {
            foreach (var unit in army)
            {
                unit.ResetWasMovedState();
            }
        }
    }
}
