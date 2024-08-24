using System.Collections.Generic;
using Units;
using UnityEngine;

namespace GamePlay.Sys
{
    public class TurnBaseSystem : MonoBehaviour
    {
        public static event System.Action<Commander> OnPhaseUpdate;

        private Board board;
        private Commander phase = Commander.NULL;
        public Commander Phase { get => phase; }

        public void Activate(Board _board)
        {
            board = _board;

            phase = DetermineStartPhase();
            OnPhaseUpdate?.Invoke(phase);
            print("Starting phase: " + phase);
        }

        void Update()
        {
            if (phase != Commander.NULL && board != null)
            {
                if (AllUnitsMoved())
                {
                    EndPhase();
                    OnPhaseUpdate?.Invoke(phase);
                }
            }

        }

        bool AllUnitsMoved()
        {
            List<Unit> currentArmy = (phase == Commander.PLAYER_1) ? board.player1Army : board.player2Army;
            foreach (Unit unit in currentArmy)
            {
                if (!unit.IsWasMoved)
                    return false;
            }
            return true;
        }

        void EndPhase()
        {
            List<Unit> currentArmy = (phase == Commander.PLAYER_1) ? board.player1Army : board.player2Army;
            ResetArmyMove(currentArmy);
            phase = (phase == Commander.PLAYER_1) ? Commander.PLAYER_2 : Commander.PLAYER_1;
            print("New phase: " + phase);
        }

        void ResetArmyMove(List<Unit> army)
        {
            foreach (Unit unit in army)
            {
                unit.ResetWasMovedState();
            }
        }

        Commander DetermineStartPhase()
        {
            return (Random.value < 0.5f) ? Commander.PLAYER_1 : Commander.PLAYER_2;
        }
    }
}
