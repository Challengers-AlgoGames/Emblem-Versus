using Units;
using UnityEngine;

namespace GamePlay.Sys {
    public class FightSystem : MonoBehaviour
    {
        private TileSystem tileSystem;
        private Grid grid;
        private float gridScale;
        private Unit currentUnit;

        public void Activate(TileSystem _tileSystem, Grid _grid)
        {
            tileSystem = _tileSystem;
            grid = _grid;
            gridScale = grid.cellSize.x;
        }

        public void DetectEnemies(Unit _unit)
        {
            currentUnit = _unit;
            Vector3 startPosition = GetGroundPositionBelow(currentUnit.transform.position);
            
            if(startPosition != Vector3.negativeInfinity)
            {
                Debug.Log("Ennemie detection");
                int minRange = currentUnit.Weapon.MinRange;
                int maxRange = currentUnit.Weapon.MaxRange;
                LocalizeTouchableEnemies(startPosition, minRange, maxRange);
            }
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

        void LocalizeTouchableEnemies(Vector3 start, int minRange, int maxRange)
        {
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
                        // Process or mark the enemy at topPosition
                        Debug.Log("Enemy found at topLeftPosition");
                    }

                    if (SearchEnemy(bottomLeftPosition)) {
                        // Process or mark the enemy at bottomPosition
                        Debug.Log("Enemy found at bottomLeftPosition");
                    }

                    if (SearchEnemy(topRightPosition)) {
                        // Process or mark the enemy at topRightPosition
                        Debug.Log("Enemy found at topRightPosition");
                    }

                    if (SearchEnemy(bottomRightPosition)) {
                        // Process or mark the enemy at bottomRightPosition
                        Debug.Log("Enemy found at bottomRightPosition");
                    }
                }
            }
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
    }
}
