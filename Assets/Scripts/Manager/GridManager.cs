using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tools
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;


        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private int levelWidth;
        [SerializeField] private int LevelHeight;
        [SerializeField] private int tileScale;


        private Dictionary<Vector3Int, GameObject> tiles;

        void Awake()
        {
            Instance = this;
        }
        // void Start()
        // {
        //     GenerateTerrain();
        // }

        public void GenerateTerrain()
        {
            tiles = new Dictionary<Vector3Int, GameObject>();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int z = 1; z < LevelHeight; z++)
                {
                    // tile scale espacement entre les tuiles
                    // axe y = 0 pour mettre les tuiles au meme niveau
                    Vector3Int tilePosition = new Vector3Int(x * tileScale, 0, z * tileScale);



                    // Instantiate pour creer une copie d'un objet dans la grill
                    //Instantiate(modele a place, position , rotation) 


                    var spawnedTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                    spawnedTile.name = $"Tile {x}{z}";

                    var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0);

                    tiles[tilePosition] = spawnedTile;
                }
            }

        }
        // public Tile GetTilePosition(Vector3 pos)
        // {
        //     if (tiles.TryGetValue(pos,))


        //         return null;
        // }
    }
}
