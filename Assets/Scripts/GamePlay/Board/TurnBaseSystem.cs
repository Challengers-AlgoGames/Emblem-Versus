using Units;
using UnityEngine;

namespace GamePlay
{
    public class TurnBaseSystem : MonoBehaviour
    {
        [SerializeField] private Unit[] manalambaUnits;
        [SerializeField] private Unit[] frenchUnits;

        private Commander phase = Commander.NULL;
        public Commander Phase { get => phase; }

        void Awake()
        {
            GameManager.OnGameStated += OnGameStated;
        }

        void OnDestroy()
        {
            GameManager.OnGameStated -= OnGameStated;
        }

        void OnGameStated()
        {
            phase = DetermineStartPhase();
            print("Starting phase: " + phase);
        }

        void Update()
        {
            if(phase != Commander.NULL)
            {
                if (AllUnitsMoved())
                {
                    EndPhase();
                }
            }
            
        }

        bool AllUnitsMoved()
        {
            Unit[] currentArmy = (phase == Commander.PLAYER_1) ? manalambaUnits : frenchUnits;
            
            foreach (Unit unit in currentArmy)
            {
                if (!unit.IsWasMoved)
                    return false;
            }
            return true;
        }

        void EndPhase()
        {
            Unit[] currentArmy = (phase == Commander.PLAYER_1) ? manalambaUnits : frenchUnits;
            ResetArmyMove(currentArmy);
            phase = (phase == Commander.PLAYER_1) ? Commander.PLAYER_2 : Commander.PLAYER_1;
            print("New phase: " + phase);
        }

        void ResetArmyMove(Unit[] army)
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
