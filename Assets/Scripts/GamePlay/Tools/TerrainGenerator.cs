using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tools {
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private int width;
        [SerializeField] private int height;

        void Start()
        {
            GenerateTerrain();
        }

        void GenerateTerrain()
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    // Position de la cellule dans la Tilemap
                    Vector3Int tilePosition = new Vector3Int(x, 0, z);

                    // Placer la tuile à la position générée
                    Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                }
            }
        }
    }
}
