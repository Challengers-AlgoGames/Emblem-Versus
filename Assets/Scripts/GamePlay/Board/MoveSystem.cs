using System;
using Units;
using UnityEngine;

namespace GamePlay {
    public class MoveSystem : MonoBehaviour
    {
        [SerializeField] private TileSystem tileSystem;
        private Action<Unit,Vector3> handleDisplayMoveRange;

        void Awake()
        {
            handleDisplayMoveRange = (unit, cellulPosition) => {
                DisplayMoveRange(unit, cellulPosition);
            };

            InputHandler.OnDisplayUnitMoveRange += handleDisplayMoveRange;
        }

        private void DisplayMoveRange(Unit unit, Vector3 cellulPosition)
        {
            Vector3Int tilePosition = tileSystem.ConvertWorldToCellPosition(cellulPosition);
            tileSystem.SetTile(tilePosition);
        }

        void OnDestroy()
        {
            InputHandler.OnDisplayUnitMoveRange -= handleDisplayMoveRange;
        }
    }
}