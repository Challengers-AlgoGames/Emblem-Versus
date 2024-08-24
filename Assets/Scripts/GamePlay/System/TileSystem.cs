using UnityEngine;
using UnityEngine.Tilemaps;

namespace GamePlay
{
    public class TileSystem : MonoBehaviour
    {
        [SerializeField] private Tile tile;
        [SerializeField] private Color tileColor;

        [Header("For Tests")]
        [SerializeField] private Vector3Int tilePosition;
        [SerializeField] private bool testMode;
        
        private Vector3Int currentTilePosition;
        private Color currentTileColor;
        private Tile tileTest;

        private Tilemap tilemap;
        public Tilemap TileMap { get => tilemap; }

        public void Active(Tilemap _tilemap)
        {
            tilemap = _tilemap;
        }

        void ManageTest()
        {
             if (testMode && tileTest == null)
            {
                tileTest = tile;
                tilemap.SetTile(tilePosition, tileTest);
                currentTilePosition = tilePosition; //

                SetTileColor(tilePosition, tileColor);
                currentTileColor = tileColor; //
            }

            if (testMode && tileTest != null)
            {
                if (currentTilePosition != tilePosition)
                {
                    MoveTile(currentTilePosition, tilePosition);
                }

                if (currentTileColor != tileColor)
                {
                    SetTileColor(currentTilePosition, tileColor);
                }
            }

            if (!testMode && tileTest != null)
            {
                tilemap.SetTile(currentTilePosition, null);
                tileTest = null;
                currentTilePosition = Vector3Int.zero;
            }
        }

        void SetTileColor(Vector3Int tilePosition, Color color)
        {
            tilemap.SetTileFlags(tilePosition, TileFlags.None);
            tilemap.SetColor(tilePosition, color);
        }

        void MoveTile(Vector3Int currentPosition, Vector3Int newPosition)
        {
            TileBase tile = tilemap.GetTile(currentPosition);

            if (tile == null) return;

            RemoveTile(currentPosition);
            tilemap.SetTile(newPosition, tile);

            currentTilePosition = newPosition;
        }

        public void SetTile(Vector3Int _tilePosition)
        {
            Tile newTile = tile;
            tilemap.SetTile(_tilePosition, newTile);
        }

        public void RemoveTile(Vector3Int _tilePosition)
        {
            tilemap.SetTile(_tilePosition, null);
        }

        public Vector3Int ConvertWorldToCellPosition(Vector3 _worldPosition)
        {
            return tilemap.WorldToCell(_worldPosition);
        }
        public Vector3 ConvertCellToWorldPosition(Vector3Int cellPosition)
        {
            // Convertit la position de la cellule en position dans le monde
            Vector3 worldPosition = tilemap.GetCellCenterWorld(cellPosition);

            return worldPosition;
        }
    }
}