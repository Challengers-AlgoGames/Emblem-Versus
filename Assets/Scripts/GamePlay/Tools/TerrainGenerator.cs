using UnityEngine;
using System.Collections.Generic;

namespace Tools
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private Transform container;
        [SerializeField] private int levelWidth = 10;
        [SerializeField] private int levelHeight = 10;
        [SerializeField] private int tileScale = 3;
        [SerializeField] private int numberOfObstacles = 10;

        private HashSet<Vector3Int> obstaclePositions = new HashSet<Vector3Int>();

        void Start()
        {
            GenerateObstacles();
            GenerateTerrain();
        }

        void GenerateObstacles()
        {
            for (int i = 0; i < numberOfObstacles; i++)
            {
                int randomX = Random.Range(0, levelWidth);
                int randomZ = Random.Range(1, levelHeight);
                Vector3Int obstaclePosition = new Vector3Int(randomX * tileScale, 0, randomZ * tileScale);

                // Vérifie si la position est déjà occupée par un obstacle
                if (!obstaclePositions.Contains(obstaclePosition) && !IsPositionOccupied(obstaclePosition))
                {
                    // Instancier l'obstacle à la position spécifiée
                    GameObject newObstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
                    newObstacle.transform.parent = container;

                    // Enregistrer la position de l'obstacle
                    obstaclePositions.Add(obstaclePosition);
                }
                else
                {
                    // Si la position est déjà occupée, réessayez avec une autre position
                    i--;
                }
            }
        }

        void GenerateTerrain()
        {
            for (int x = 0; x < levelWidth; x++)
            {
                for (int z = 1; z < levelHeight; z++)
                {
                    Vector3Int tilePosition = new Vector3Int(x * tileScale, 0, z * tileScale);

                    // Place la tuile uniquement si la position n'est pas occupée par un obstacle
                    if (!obstaclePositions.Contains(tilePosition))
                    {
                        GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                        newTile.transform.parent = container;
                    }
                }
            }
        }

        bool IsPositionOccupied(Vector3Int position)
        {
            // Vérifie si la position est occupée par un obstacle
            return obstaclePositions.Contains(position);
        }
    }
}
