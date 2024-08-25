using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GamePlay {

    [RequireComponent(typeof(Grid))]
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
        [SerializeField] private float unitHeighPosition = 1.27f;

        [Header("Units Parameters")]
        [SerializeField] private Transform player1UnitsContainer;
        [SerializeField] private Transform player2UnitsContainer;

        private Grid grid;
        private Tilemap tilemap;
        private List<Unit> player1Units;
        private List<Unit> player2Units;

        public List<Unit> Player1Army { get => player1Units; }
        public List<Unit> Player2Army { get => player2Units; }

        /* "o" : water, "" : grass, "T" : tree, "t" : deathTree */
        /* "cu" : cuirassier, "hu" : hussard, "if" : infanterie */ // unités camp 1
        /* "mzt" : mapndefa zanatsipika, "mad" : mpanao ady, "mdb" : mpadatsa baratra */ // unités camp2
        private string[,] map = {
            {"","", "", "", "",  "",  "",  "",  "T",  "T", "T",  "",  "",  "",  "",  "", "T", "t", "T", "T"},
            {"", "",  "",  "T",  "",  "",  "T", "",  "",  "",  "",  "T",  "",  "",  "",  "",  "",  "",  "",  "T"},
            {"", "T", "",  "",  "",  "",  "T",  "",  "",  "t",  "",  "",  "",  "T",  "", "",  "",  "T",  "",  "T"},
            {"", "",  "",  "", "T",  "",  "",  "",  "",  "T",  "",  "",  "",  "",  "T",  "T",  "",  "",  "",  "T"},
            {"",  "o",  "o", "T",  "",  "",  "T", "",  "",  "t",  "",  "",  "",  "",  "",  "",  "o",  "",  "o",  "T"},
            {"",  "",  "T",  "",  "",  "o", "T",  "",  "",  "T",  "",  "",  "",  "",  "T",  "",  "",  "",  "",  "t"},
            {"T",  "",  "",  "",  "",  "",  "",  "",  "T",  "",  "T",  "",  "T",  "",  "",  "",  "",  "",  "T",  "T"},
            {"",  "",  "T",  "T",  "",  "",  "",  "",  "",  "",  "",  "",  "o", "",  "",  "T",  "",  "T",  "T",  "T"},
            {"",  "",  "T",  "o",  "o", "T",  "",  "",  "T",  "",  "",  "",  "",  "",  "o", "",  "",  "",  "",  "t"},
            {"",  "",  "",  "o",  "",  "T",  "",  "",  "",  "T",  "",  "T",  "",  "",  "",  "T",  "",  "",  "",  "T"},
            {"",  "",  "T",  "",  "",  "T",  "",  "T",  "",  "",  "T",  "T",  "",  "",  "", "",  "",  "",  "",  ""},
            {"T",  "",  "T",  "",  "",  "",  "",  "T", "T",  "",  "T",  "",  "",  "",  "",  "t",  "",  "", "T",  ""},
            {"", "",  "",  "",  "",  "T",  "",  "",  "T",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  ""},
            {"T", "", "", "T", "",  "",  "",  "",  "",  "T",  "",  "",  "T",  "",  "",  "T", "", "t", "T", "T"},
        };

        private string[,] campsFormation = {
            {"","", "", "", "",  "",  "",  "",  "",  "", "",  "",  "",  "",  "",  "", "", "", "", ""},
            {"", "",  "mzt",  "",  "",  "",  "", "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  ""},
            {"", "", "mad",  "mzt",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "", "",  "if",  "",  "",  ""},
            {"mdb", "",  "",  "mzt", "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "cu",  "",  "hu",  ""},
            {"mad",  "mdb",  "", "",  "",  "",  "", "",  "",  "",  "",  "",  "",  "",  "",  "",  "if",  "",  "",  ""},
            {"",  "mad",  "",  "",  "",  "", "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "hu",  "if",  "hu",  ""},
            {"",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "cu",  "",  ""},
            {"",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "", "",  "",  "",  "",  "",  "",  ""},
            {"",  "",  "",  "",  "", "",  "",  "",  "",  "",  "",  "",  "",  "",  "", "",  "",  "",  "",  ""},
            {"",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  ""},
            {"",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "", "",  "",  "",  "",  ""},
            {"",  "",  "",  "",  "",  "",  "",  "", "",  "",  "",  "",  "",  "",  "",  "",  "",  "", "",  ""},
            {"", "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  ""},
            {"", "", "", "", "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "",  "", "", "", "", ""},
        };

        void Awake()
        {
            grid = GetComponent<Grid>();
            tilemap = GetComponentInChildren<Tilemap>();

            player1Units = new List<Unit>();
            player2Units = new List<Unit>();
        }

        public void Generate()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    GameObject newTile = InstantiateTile(map[i, j], new Vector3Int(i * tileScale, 0, j * tileScale));
                    newTile.transform.parent = container;
                }
            }            
        }

        private GameObject InstantiateTile(string tileType, Vector3Int position)
        {
            switch(tileType)
            {
                case "o":
                    return Instantiate(waterTilePrefab, position, Quaternion.identity);
                case "T":
                    return Instantiate(forestTilePrefab, position, Quaternion.identity);
                case "t":
                    return Instantiate(deathForestTilePrefab, position, Quaternion.identity);
                default:
                    return Instantiate(gassTilePrefab, position, Quaternion.identity);
            }
        }

        public void PlaceUnits()
        {
            for (int i = 0; i < campsFormation.GetLength(0); i++)
            {
                for (int j = 0; j < campsFormation.GetLength(1); j++)
                {
                    Vector3 unitPosition = new Vector3(i * tileScale, unitHeighPosition, j * tileScale);
                    GameObject newUnit = InstantiateUnit(campsFormation[i, j], unitPosition);
                    if (newUnit != null) SetNewUnitInContainer(newUnit);
                }
            }
        }

        private GameObject InstantiateUnit(string unitType, Vector3 position)
        {
            switch(unitType)
            {
                case "cu":
                    return Instantiate(cuirassierPrefab, position, Quaternion.Euler(0, 180, 0));
                case "hu":
                    return Instantiate(hyssardPrefab, position, Quaternion.Euler(0, 180, 0));
                case "if":
                    return Instantiate(infanryPrefab, position, Quaternion.Euler(0, 180, 0));
                case "mzt":
                    return Instantiate(mpandefaZanatsipikaPrefab, position, Quaternion.identity);
                case "mad":
                    return Instantiate(mpanaoAdyPrefab, position, Quaternion.identity);
                case "mdb":
                    return Instantiate(mpandatsaBaratraPrefab, position, Quaternion.identity);
                default:
                    return null;
            }
        }

        void SetNewUnitInContainer(GameObject unit)
        {
            if(unit.GetComponent<Unit>().Commander == Commander.PLAYER_1)
            {
                unit.transform.parent = player1UnitsContainer;
                player1Units.Add(unit.GetComponent<Unit>());
            }
            else
            {
                unit.transform.parent = player2UnitsContainer;
                player2Units.Add(unit.GetComponent<Unit>());
            }
        }

        public Grid GetGrid()
        {
            return grid;
        }

        public Tilemap GetTilemap()
        {
            return tilemap;
        }

        public void ElimineUnit(Unit _unit)
        {
            if(_unit.Commander == Commander.PLAYER_1)
            {
                Player1Army.Remove(_unit);
            } else {
                Player2Army.Remove(_unit);
            }

            Destroy(_unit.gameObject);
        }
    }
}