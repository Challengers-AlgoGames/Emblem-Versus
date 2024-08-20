using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace GamePlay
{
    public class MoveSystem : MonoBehaviour
    {
        private Action<Unit, Vector3> handleDisplayMoveRange;
        private Action<Vector3> handleMoveUnit;
        private List<Vector3Int> activeTilesPosition;

        [SerializeField] private TileSystem tileSystem;

        [Tooltip("Tiles scale. Support only square tiles")]
        [SerializeField] private float scale = 3f;
        [SerializeField] private PathfindingAStar pathfindingAStar;

        private Unit unit;

        void Awake()
        {
            handleDisplayMoveRange = (unit, cellulPosition) =>
            {
                DisplayMoveRange(unit, cellulPosition);
            };

            handleMoveUnit = (targetPosition) =>
            {
                MoveUnit(targetPosition);
            };

            InputHandler.OnDisplayUnitMoveRange += handleDisplayMoveRange;
            InputHandler.OnMoveUnit += handleMoveUnit;
            InputHandler.OnClearUI += ClearActiveTiles;
        }

        void Start()
        {
            activeTilesPosition = new List<Vector3Int>();
        }

        void ClearActiveTiles()
        {
            if (activeTilesPosition == null) return; // Do nothing if not active tiles

            // Remove active tiles
            foreach (Vector3Int position in activeTilesPosition)
            {
                tileSystem.RemoveTile(position);
            }

            ClearActiveTilePositionData();
        }

        void MoveUnit(Vector3 _targetPosition)
        {
            // Convertir les positions de l'unité et de la cible en coordonnées de cellules
            Vector3Int start = tileSystem.ConvertWorldToCellPosition(unit.transform.position);
            Vector3Int end = tileSystem.ConvertWorldToCellPosition(_targetPosition);

            Debug.Log(start + " " + end);

            // Trouver le chemin en utilisant A*
            List<Vector3Int> path = pathfindingAStar.FindPath(start, end);

            // Afficher les points du chemin pour débogage
            Debug.Log("Path found:");
            foreach (Vector3Int point in path)
            {
                Debug.Log(point);
            }

            // Si le chemin contient des points, démarrer le déplacement le long du chemin
            if (path.Count > 0)
            {
                Debug.Log(unit.transform.position);
                StartCoroutine(MoveUnitAlongPath(path));
                Debug.Log("After move");
            }
            else
            {
                Debug.Log("No path found.");
            }
        }


        IEnumerator MoveUnitAlongPath(List<Vector3Int> path)
        {
            if (path == null || path.Count == 0)
            {
                yield break; // Sortir si le chemin est vide
            }

            // Retirer la position initiale du chemin
            if (path.Count > 0)
            {
                path.RemoveAt(0);
            }
            foreach (Vector3Int point in path)
            {
                Vector3 targetPosition = tileSystem.ConvertCellToWorldPosition(point);
                Debug.Log("Moving to position: " + targetPosition);
                Debug.Log("Unit: " + unit);
                Debug.Log("Tyle system: " + tileSystem);
                unit.Move(targetPosition);

                // unit.StartMoving(targetPosition); // Utilisez la méthode pour démarrer le mouvement

                // // Attendre que l'unité arrive à la position cible avant de passer au suivant
                // while (unit.IsMoving)
                // {
                //     // Comparer la position de l'unité avec la position cible
                //     if (Vector3.Distance(unit.transform.position, targetPosition) < 0.1f)
                //     {
                //         unit.StopMoving(); // Utilisez la méthode pour arrêter le mouvement
                //     }

                //     yield return null; // Attendre le prochain frame
                // }

                // // Ajouter une petite pause pour éviter un mouvement trop rapide
                yield return new WaitForSeconds((float)0.5);
            }

            // Optionnel : Marquer l'unité comme ayant été déplacée
            // unit.Wait();
            yield return null;
        }




        void ClearActiveTilePositionData()
        {
            activeTilesPosition = new List<Vector3Int>();
            // unit = null;
        }

        void DisplayMoveRange(Unit _unit, Vector3 _unitPosition)
        {
            unit = _unit;

            if (unit.IsWasMoved) return; // dont show if unit was moved

            int unitMobility = unit.Mobility;

            for (int line = 0; line <= unitMobility + 1; line++)
            {
                // lines
                Vector3 topLineVector = _unitPosition + new Vector3(0, 0, (unitMobility + 1 - line) * scale);
                Vector3 bottomLineVector = _unitPosition + new Vector3(0, 0, (-unitMobility - 1 + line) * scale);

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
        }

        void OnDestroy()
        {
            InputHandler.OnDisplayUnitMoveRange -= handleDisplayMoveRange;
            InputHandler.OnMoveUnit -= handleMoveUnit;
            InputHandler.OnClearUI -= ClearActiveTiles;
        }
    }
}
