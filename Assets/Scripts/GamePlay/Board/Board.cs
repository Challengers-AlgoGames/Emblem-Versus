using Units;
using UnityEngine;

namespace GamePlay {
    public class Board : MonoBehaviour
    {
        [Header("Tiles Prefab")]
        [SerializeField] private GameObject gassTilePrefab;
        [SerializeField] private GameObject waterTilePrefab;
        [SerializeField] private GameObject forestTilePrefab;
        [SerializeField] private GameObject deathForestTilePrefab;

        [Header("Units Prefab")]
        [SerializeField] private GameObject cuirassierPrefab;
        [SerializeField] private GameObject hyssardPrefab;
        [SerializeField] private GameObject infanryPrefab;
        [SerializeField] private GameObject mpandefaZanatsipikaPrefab;
        [SerializeField] private GameObject mpanaoAdyPrefab;
        [SerializeField] private GameObject mpandatsaBaratraPrefab;

        [Header("Terrain parameters")]
        [SerializeField] private Transform container;
        [SerializeField] private int tileScale = 3;

        /* "o" : water, "" : grass, "T" : tree, "t" : deathTree */
        /* "cu" : cuirassier, "hu" : hussard, "if" : infanterie */ // unités camp 1
        /* "mzt" : mapndefa zanatsipika, "mad" : mpanao ady, "mdb" : mpadatsa baratra */ // unités camp2
        private string[,] map = {
            {"if", "ch", "if", "", "",  "",  "",  "",  "T",  "T", "T",  "",  "",  "",  "",  "", "T", "T", "T", ""},
            {"cu", "hu",  "ch",  "T",  "",  "",  "T", "",  "",  "",  "",  "T",  "",  "",  "",  "",  "T",  "",  "",  ""},
            {"hu",  "T", "cu",  "T",  "",  "",  "T",  "",  "",  "t",  "T",  "",  "",  "T",  "T", "",  "T",  "T",  "T",  ""},
            {"T", "",  "",  "T", "T",  "",  "",  "",  "",  "T",  "",  "",  "",  "",  "T",  "T",  "",  "T",  "",  ""},
            {"",  "T",  "o", "T",  "",  "",  "T", "",  "",  "T",  "",  "",  "",  "",  "",  "",  "o",  "",  "T",  ""},
            {"",  "",  "T",  "",  "",  "o", "T",  "",  "",  "T",  "",  "T",  "",  "",  "T",  "",  "",  "",  "T",  ""},
            {"T",  "",  "",  "T",  "",  "",  "",  "",  "T",  "",  "T",  "",  "T",  "",  "",  "T",  "",  "T",  "T",  ""},
            {"",  "",  "T",  "T",  "",  "",  "",  "",  "",  "",  "",  "T",  "o", "",  "",  "T",  "",  "T",  "T",  ""},
            {"T",  "",  "T",  "",  "o", "T",  "",  "",  "T",  "",  "",  "T",  "",  "",  "o", "T",  "",  "T",  "T",  "T"},
            {"T",  "",  "T",  "",  "",  "T",  "",  "",  "T",  "T",  "",  "T",  "",  "",  "",  "T",  "",  "T",  "T",  ""},
            {"T",  "T",  "T",  "",  "",  "T",  "",  "T",  "T",  "",  "T",  "T",  "",  "",  "T", "mzt",  "",  "T",  "",  ""},
            {"T",  "",  "T",  "",  "",  "",  "",  "T", "T",  "",  "T",  "T",  "",  "mad",  "mad",  "T",  "T",  "t", "T",  "T"},
            {"", "T",  "T",  "T",  "",  "T",  "",  "",  "T",  "",  "",  "T",  "",  "mad",  "mzt",  "mzt",  "",  "T",  "T",  ""},
            {"", "", "T", "T", "T",  "T",  "",  "",  "",  "T",  "",  "",  "T",  "",  "mdb",  "T", "mdb", "T", "T", "T"},
        };

        public void Generate()
        {
            for (int i = 0; i < map.GetLength(0); i++)  // Boucle sur les lignes
            {
                for (int j = 0; j < map.GetLength(1); j++)  // Boucle sur les colonnes
                {
                    GameObject newTile;
                    string cellContent = map[i, j];
                    Vector3Int tilePosition = new Vector3Int(i * tileScale, 0, j * tileScale);
                    
                    switch(cellContent)
                    {
                        case "o":
                            newTile = Instantiate(waterTilePrefab, tilePosition, Quaternion.identity);
                            break;
                        case "T":
                            newTile = Instantiate(forestTilePrefab, tilePosition, Quaternion.identity);
                            break;
                        case "t":
                            newTile = Instantiate(deathForestTilePrefab, tilePosition, Quaternion.identity);
                            break;
                        default:
                            newTile = Instantiate(gassTilePrefab, tilePosition, Quaternion.identity);
                            break;
                    }

                    newTile.transform.parent = container; // set parent
                }
            }            
        }
    }
}
