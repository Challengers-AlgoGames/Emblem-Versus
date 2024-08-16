using UnityEngine;

namespace Tools {
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Transform container;
        [SerializeField] private int levelWidth;
        [SerializeField] private int levelHeight;
        [SerializeField] private int tileScale;


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
