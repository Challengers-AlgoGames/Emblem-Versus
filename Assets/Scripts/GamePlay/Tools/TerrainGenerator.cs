using UnityEngine;

namespace Tools {
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Transform container;
        [SerializeField] private int levelWidth = 10;
        [SerializeField] private int levelHeight = 10;
        [SerializeField] private int tileScale = 3;


        void Start()
        {
            GenerateTerrain();
        }

        void GenerateTerrain()
        {
            for(int x = 0; x < levelWidth; x++)
            {
                for(int z = 1; z < levelHeight; z++)
                {
                    Vector3Int tilePosition = new Vector3Int(x * tileScale, 0, z * tileScale);
                    GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                    newTile.transform.parent = container;
                }

            }
            
        }
    }
}
