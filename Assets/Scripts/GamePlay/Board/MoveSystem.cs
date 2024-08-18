using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace GamePlay {
    public class MoveSystem : MonoBehaviour
    {
        [SerializeField] private TileSystem tileSystem;

        [Tooltip("Tiles scale. Support only square tiles")]
        [SerializeField] private float scale = 3f;

        private Action<Unit,Vector3> handleDisplayMoveRange;
        private Action<Vector3> handleMoveUnit;

        private List<Vector3Int> activeTilesPosition;
        private Unit unit;

        void Awake()
        {
            handleDisplayMoveRange = (unit, cellulPosition) => {
                DisplayMoveRange(unit, cellulPosition);
            };

            handleMoveUnit = (targetPosition) => {
                MoveUnit(targetPosition);
            };

            InputHandler.OnDisplayUnitMoveRange += handleDisplayMoveRange;
            InputHandler.OnMoveUnit += handleMoveUnit;
            InputHandler.OnResetUnitMove += ResetUnitMove;
            InputHandler.OnClearUI += ClearAtiveTiles;
        }

        void Start()
        {
            activeTilesPosition = new List<Vector3Int>();
        }

        void ClearAtiveTiles()
        {
            if(activeTilesPosition == null) return; // Do nothing if not active tiles

            // Remove active tiles
            foreach (Vector3Int position in activeTilesPosition)
            {
                tileSystem.RemoveTile(position);
            }
            
            ClearActiveTilePositionData();
        }

        void MoveUnit(Vector3 _targetPosition)
        {
            Debug.Log("Move to: " + _targetPosition);
        }

        void ResetUnitMove()
        {
            Debug.Log("Move canceled");
        }

        void ClearActiveTilePositionData()
        {
            activeTilesPosition = new List<Vector3Int>();
            unit = null;
        }

        void DisplayMoveRange(Unit _unit, Vector3 _unitCellulPosition)
        {
            unit = _unit;

            if(unit.IsWasMoved) return; // dont show if unit was moved

            int unitMobility = unit.Mobility;
            for(int line = 0; line <= unitMobility; line++) // line
            {
                // positionnnements
                Vector3 lineVector = _unitCellulPosition + new Vector3(0, 0, (unitMobility - line) * scale);
                
                // left col
                for(int col = 0; col < line ; col++) // left col
                {
                    Vector3 colVector = new Vector3(-col * scale, 0, 0);

                    Vector3 cellVectorPosition = lineVector + colVector;

                    Vector3Int tilePosition = tileSystem.ConvertWorldToCellPosition(cellVectorPosition);
                    tileSystem.SetTile(tilePosition);

                    activeTilesPosition.Add(tilePosition); // save active tile in memory
                }

                // right col
                for(int col = 0; col < line ; col++)
                {
                    Vector3 colVector = new Vector3(col * scale, 0, 0);

                    Vector3 cellVectorPosition = lineVector + colVector;

                    Vector3Int tilePosition = tileSystem.ConvertWorldToCellPosition(cellVectorPosition);
                    tileSystem.SetTile(tilePosition);

                   activeTilesPosition.Add(tilePosition); // save active tile in memory
                }

            }
        }

        void OnDestroy()
        {
            InputHandler.OnDisplayUnitMoveRange -= handleDisplayMoveRange;
            InputHandler.OnMoveUnit -= handleMoveUnit;
            InputHandler.OnClearUI -= ClearAtiveTiles;
        }
    }
}