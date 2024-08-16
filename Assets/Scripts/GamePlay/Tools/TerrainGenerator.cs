using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tools {
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private int levelWidth;
        [SerializeField] private int LevelHeight;
        [SerializeField] private int tileScale;


        void Start()
        {
            GenerateTerrain();
        }

        void GenerateTerrain()
        {
            for(int x = 0; x < levelWidth; x++)
            {
                for(int z = 1; z < LevelHeight; z++)
                {
                    Vector3Int tilePosition = new Vector3Int(x * tileScale, 0, z * tileScale);
                    Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                }
            }
            
        }
    }
}
