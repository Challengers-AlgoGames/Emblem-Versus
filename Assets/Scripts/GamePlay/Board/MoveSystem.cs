using System;
using Units;
using UnityEngine;

namespace GamePlay {
    public class MoveSystem : MonoBehaviour
    {
        [SerializeField] private TileSystem tileSystem;

        [Tooltip("Tiles scale. Support only square tiles")]
        [SerializeField] private float scale = 3f;
        private Action<Unit,Vector3> handleDisplayMoveRange;

        void Awake()
        {
            handleDisplayMoveRange = (unit, cellulPosition) => {
                DisplayMoveRange(unit, cellulPosition);
            };

            InputHandler.OnDisplayUnitMoveRange += handleDisplayMoveRange;
        }

        private void DisplayMoveRange(Unit _unit, Vector3 _cellulPosition)
        {
            int unitMobility = _unit.Mobility;
            for(int line = 0; line <= unitMobility; line++) // line
            {
                // positionnnements
                Vector3 lineVector = _cellulPosition + new Vector3(0, 0, (unitMobility - line) * scale);
                
                // left col
                for(int col = 0; col < line ; col++) // left col
                {
                    Vector3 colVector = new Vector3(-col * scale, 0, 0);

                    Vector3 cellVectorPosition = lineVector + colVector;

                    Vector3Int tilePosition = tileSystem.ConvertWorldToCellPosition(cellVectorPosition);
                    tileSystem.SetTile(tilePosition);
                }

                // right col
                for(int col = 0; col < line ; col++)
                {
                    Vector3 colVector = new Vector3(col * scale, 0, 0);

                    Vector3 cellVectorPosition = lineVector + colVector;

                    Vector3Int tilePosition = tileSystem.ConvertWorldToCellPosition(cellVectorPosition);
                    tileSystem.SetTile(tilePosition);
                }

            }
        }

        void OnDestroy()
        {
            InputHandler.OnDisplayUnitMoveRange -= handleDisplayMoveRange;
        }
    }
}