using System.Collections.Generic;
using Tools;
using Units;
using UnityEngine;

namespace GamePlay.Sys
{
    [RequireComponent(typeof(TileSystem))]
    public class MoveSystem : MonoBehaviour
    {
        private float gridScale;
        private TileSystem tileSystem;
        private PathfindingAStar pathfindingAStar;
        private List<Vector3Int> activeTilesPosition;

        public void Activate(Grid _grid)
        {
            gridScale = _grid.cellSize.x;
            tileSystem = GetComponent<TileSystem>();
            pathfindingAStar = new PathfindingAStar(tileSystem);
            activeTilesPosition = new List<Vector3Int>();
        }

        public void ClearActiveTiles()
        {
            if (activeTilesPosition == null) return;

            foreach (Vector3Int position in activeTilesPosition)
            {
                tileSystem.RemoveTile(position);
            }

            activeTilesPosition = new List<Vector3Int>();
        }

        public void MoveUnit(Unit _unit, Vector3 _targetPosition)
        {
            Vector3Int start = tileSystem.ConvertWorldToCellPosition(_unit.transform.position);
            Vector3Int end = tileSystem.ConvertWorldToCellPosition(_targetPosition);
            bool IsConsideringObstacle = true;

            List<Vector3Int> path = pathfindingAStar.FindPath(start, end, IsConsideringObstacle);

            if (path.Count > 0)
            {
                path.RemoveAt(0);

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

        public void DisplayMoveRange(int _unitMobility, Vector3 _unitGroundCellPosition)
        {
            for (int line = 0; line <= _unitMobility + 1; line++)
            {
                // lines
                Vector3 topLineVector = _unitGroundCellPosition + new Vector3(0, 0, (_unitMobility + 1 - line) * gridScale);
                Vector3 bottomLineVector = _unitGroundCellPosition + new Vector3(0, 0, (-_unitMobility - 1 + line) * gridScale);

                // left cols
                for (int col = 0; col < line; col++)
                {
                    Vector3 colVector = new Vector3(-col * gridScale, 0, 0);

                    Vector3 cellTopVectorPosition = topLineVector + colVector;
                    Vector3 cellBottomVectorPosition = bottomLineVector + colVector;

                    if (Ground.VerifieWalkability(cellTopVectorPosition))
                    {
                        Vector3Int topTilePosition = tileSystem.ConvertWorldToCellPosition(cellTopVectorPosition);
                        tileSystem.SetTile(topTilePosition);
                        activeTilesPosition.Add(topTilePosition);
                    }

                    if (Ground.VerifieWalkability(cellBottomVectorPosition))
                    {
                        Vector3Int bottomTilePosition = tileSystem.ConvertWorldToCellPosition(cellBottomVectorPosition);
                        tileSystem.SetTile(bottomTilePosition);
                        activeTilesPosition.Add(bottomTilePosition);
                    }
                }

                // right cols
                for (int col = 0; col < line; col++)
                {
                    Vector3 colVector = new Vector3(col * gridScale, 0, 0);

                    Vector3 cellTopVectorPosition = topLineVector + colVector;
                    Vector3 cellBottomVectorPosition = bottomLineVector + colVector;

                    if (Ground.VerifieWalkability(cellTopVectorPosition))
                    {
                        Vector3Int topTilePosition = tileSystem.ConvertWorldToCellPosition(cellTopVectorPosition);
                        tileSystem.SetTile(topTilePosition);
                        activeTilesPosition.Add(topTilePosition);
                    }

                    if (Ground.VerifieWalkability(cellBottomVectorPosition))
                    {
                        Vector3Int bottomTilePosition = tileSystem.ConvertWorldToCellPosition(cellBottomVectorPosition);
                        tileSystem.SetTile(bottomTilePosition);
                        activeTilesPosition.Add(bottomTilePosition);
                    }
                }
            }
        }
    }
}