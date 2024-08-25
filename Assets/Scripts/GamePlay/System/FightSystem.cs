using System.Collections.Generic;
using System.Linq;
using Tools;
using Units;
using UnityEngine;

namespace GamePlay.Sys {
    public class FightSystem : MonoBehaviour
    {
        private TileSystem tileSystem;
        private PathfindingAStar pathfindingAStar;
        private Grid grid;
        private float gridScale;
        private Unit currentUnit;

        public void Activate(TileSystem _tileSystem, Grid _grid)
        {
            tileSystem = _tileSystem;
            grid = _grid;
            gridScale = grid.cellSize.x;

            pathfindingAStar = new PathfindingAStar(tileSystem);
        }

        public List<Vector3Int> DetectEnemies(Unit _unit)
        {
            currentUnit = _unit;
            Vector3 startPosition = GetGroundPositionBelow(currentUnit.transform.position);
            
            List<Vector3Int> touchableEnnemiesCellPos = new List<Vector3Int>();

            if(startPosition != Vector3.negativeInfinity)
            {
                Debug.Log("Ennemie detection");
                int minRange = currentUnit.Weapon.MinRange;
                int maxRange = currentUnit.Weapon.MaxRange;

                touchableEnnemiesCellPos = SelectTouchable(startPosition, minRange, maxRange, LocalizeEnemies(startPosition, maxRange));
            }

            if(touchableEnnemiesCellPos.Count > 0)
            {
                foreach (var item in touchableEnnemiesCellPos)
                {
                    Debug.Log("touchable ennemi pos :" + item);
                }
            }

            return touchableEnnemiesCellPos;
        }

        Vector3 GetGroundPositionBelow(Vector3 unitPosition)
        {
            RaycastHit hit;
            Physics.Raycast(new Vector3(unitPosition.x, -4f, unitPosition.z), Vector3.up, out hit);

            if(hit.collider)
            {
                if(hit.collider.CompareTag("Ground"))
                {
                    return hit.collider.gameObject.transform.position;
                } 
                else 
                {
                    Debug.Log("Not a ground");
                }
            } 
            else 
            {
                Debug.Log("No collider was touched");
            }

            return Vector3.negativeInfinity;
        }

        List<Vector3> LocalizeEnemies(Vector3 start, int maxRange)
        {
            List<Vector3> enemiesPosition = new List<Vector3>();

            Debug.Log("Ennemie localization");

            for (int line = 0; line <= maxRange; line++)
            {
                Vector3 topLineDirection = start + new Vector3(0, 0, (maxRange - line) * gridScale); // montée
                Vector3 bottomLineDirection = start + new Vector3(0, 0, (-maxRange + line) * gridScale); // descente
            
                // left cols
                for (int col = 0; col <= line; col++)
                {
                    Vector3 colDirection = new Vector3(-col * gridScale, 0, 0);

                    Vector3 topLeftPosition = topLineDirection + colDirection;
                    Vector3 bottomLeftPosition = bottomLineDirection + colDirection;

                    // Right cols
                    Vector3 rightColDirection = new Vector3(col * gridScale, 0, 0);

                    Vector3 topRightPosition = topLineDirection + rightColDirection;
                    Vector3 bottomRightPosition = bottomLineDirection + rightColDirection;

                    // Check for enemies in each direction
                    if (SearchEnemy(topLeftPosition)) {
                        enemiesPosition.Add(topLeftPosition);
                    }

                    if (SearchEnemy(bottomLeftPosition)) {
                        enemiesPosition.Add(bottomLeftPosition);
                    }

                    if (SearchEnemy(topRightPosition)) {
                        enemiesPosition.Add(topRightPosition);
                    }

                    if (SearchEnemy(bottomRightPosition)) {
                        enemiesPosition.Add(bottomRightPosition);
                    }
                }
            }

            if(enemiesPosition.Count > 1)
            {
                enemiesPosition = enemiesPosition.Distinct<Vector3>().ToList<Vector3>();
            }

            return enemiesPosition; 
        }

        bool SearchEnemy(Vector3 position)
        {
            if(Physics.Raycast(new Vector3(position.x, 4f, position.z), Vector3.down, out RaycastHit hit))
            {
                if(hit.collider.CompareTag("Unit"))
                {
                    Unit detectedUnit = hit.collider.GetComponent<Unit>();
                    if(detectedUnit.Commander != currentUnit.Commander)
                        return true;
                }
            }

            return false;
        }

        public List<Vector3Int> SelectTouchable(Vector3 startPosition, int minRange, int maxRange, List<Vector3> enemiesPosition)
        {
            List<Vector3Int> touchableEnemiesCellPosition = new List<Vector3Int>();

            foreach (Vector3 position in enemiesPosition)
            {
                Vector3Int start = tileSystem.ConvertWorldToCellPosition(startPosition);
                Vector3Int target = tileSystem.ConvertWorldToCellPosition(position);

                // Utilisation de la fonction FindPath en ignorant les obstacles
                List<Vector3Int> pathToEnemy = pathfindingAStar.FindPath(start, target, false);
                foreach (Vector3Int path in pathToEnemy)
                {
                    Debug.Log(path);
                }

                // Si le chemin est vide, cela signifie qu'il n'y a pas de chemin valide
                if (pathToEnemy == null || pathToEnemy.Count == 0)
                    continue;
                Debug.Log("Continue");


                // Calcul de la distance du chemin
                int distance = pathToEnemy.Count - 1;

                // Vérification si l'ennemi est dans la portée d'attaque
                if (distance <= maxRange && distance >= minRange)
                {
                    Debug.Log("ajout");

                    // Ajout des coordonnées de l'ennemi à la liste des ennemis attaquables
                    touchableEnemiesCellPosition.Add(target);
                }
            }

            return touchableEnemiesCellPosition;
        }
    }
}
