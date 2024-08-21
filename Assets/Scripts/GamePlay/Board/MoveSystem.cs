using System;
using System.Collections.Generic;
using Tools;
using Units;
using UnityEngine;

namespace GamePlay
{
    public class MoveSystem : MonoBehaviour
    {
        [SerializeField] private TileSystem tileSystem;
        [Tooltip("Tiles scale. Support only square tiles")]
        [SerializeField] private float scale = 3f;
        [SerializeField] private PathfindingAStar pathfindingAStar;

        /* camera events */
        public static event Action<Vector3> OnDisplayMoveRange;
        public static event Action OnClearActiveTile;

        private List<Vector3Int> activeTilesPosition;

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
            activeTilesPosition = new List<Vector3Int>();
        }

        public void ClearActiveTiles()
        {
            if (activeTilesPosition == null) return; // Do nothing if not active tiles
            // Remove active tiles
            foreach (Vector3Int position in activeTilesPosition)
            {
                tileSystem.RemoveTile(position);
            }
            ClearActiveTilePositionData();
            OnClearActiveTile?.Invoke();
        }

        public void MoveUnit(Unit _unit, Vector3 _targetPosition)
        {
            // Convertir les positions de l'unité et de la cible en coordonnées de cellules
            Vector3Int start = tileSystem.ConvertWorldToCellPosition(_unit.transform.position);
            Vector3Int end = tileSystem.ConvertWorldToCellPosition(_targetPosition);

            // Trouver le chemin en utilisant A*
            List<Vector3Int> path = pathfindingAStar.FindPath(start, end);

            // Si le chemin contient des points, démarrer le déplacement le long du chemin
            if (path.Count > 0)
            {
                path.RemoveAt(0); // Retirer la position initiale du chemin

                List<Vector3> worldPath = new List<Vector3>();
                foreach (Vector3Int point in path)
                {
                    worldPath.Add(tileSystem.ConvertCellToWorldPosition(point));
                }
                
                _unit.Move(worldPath);
            }
            else
            {
                Debug.Log("No path found.");
            }
        }

        void ClearActiveTilePositionData()
        {
            activeTilesPosition = new List<Vector3Int>();
        }

        public void DisplayMoveRange(int _unitMobility, Vector3 _unitGroundCellPosition)
        {
            for (int line = 0; line <= _unitMobility + 1; line++)
            {
                // lines
                Vector3 topLineVector = _unitGroundCellPosition + new Vector3(0, 0, (_unitMobility + 1 - line) * scale);
                Vector3 bottomLineVector = _unitGroundCellPosition + new Vector3(0, 0, (-_unitMobility - 1 + line) * scale);

                // left cols
                for (int col = 0; col < line; col++)
                {
                    Vector3 colVector = new Vector3(-col * scale, 0, 0);

                    Vector3 cellTopVectorPosition = topLineVector + colVector;
                    Vector3 cellBottomVectorPosition = bottomLineVector + colVector;

                    Vector3Int topTilePosition = tileSystem.ConvertWorldToCellPosition(cellTopVectorPosition);
                    Vector3Int bottomTilePosition = tileSystem.ConvertWorldToCellPosition(cellBottomVectorPosition);

                    tileSystem.SetTile(topTilePosition);
                    tileSystem.SetTile(bottomTilePosition);

                    // save active tile in memory
                    activeTilesPosition.Add(topTilePosition);
                    activeTilesPosition.Add(bottomTilePosition);
                }

                // right cols
                for (int col = 0; col < line; col++)
                {
                    Vector3 colVector = new Vector3(col * scale, 0, 0);

                    Vector3 cellTopVectorPosition = topLineVector + colVector;
                    Vector3 cellBottomVectorPosition = bottomLineVector + colVector;

                    Vector3Int topTilePosition = tileSystem.ConvertWorldToCellPosition(cellTopVectorPosition);
                    Vector3Int bottomTilePosition = tileSystem.ConvertWorldToCellPosition(cellBottomVectorPosition);

                    tileSystem.SetTile(topTilePosition);
                    tileSystem.SetTile(bottomTilePosition);

                    // save active tile in memory
                    activeTilesPosition.Add(topTilePosition);
                    activeTilesPosition.Add(bottomTilePosition);
                }
            }
            OnDisplayMoveRange?.Invoke(_unitGroundCellPosition);
        }
    }
}