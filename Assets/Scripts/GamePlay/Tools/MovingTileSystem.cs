using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingTileSystem : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;
    [SerializeField] private Color tileColor = new Color(21,88,197,100);
    [SerializeField] private Vector3Int tilePosition;
    private Vector3Int currentTilePosition;
    private Color currentTileColor;

    void Start()
    {
        Tile newTile = tile;
        
        tilemap.SetTile(tilePosition, newTile);
        currentTilePosition = tilePosition; //

        SetTileColor(tilePosition, tileColor);
        currentTileColor = tileColor; //
    }

    void Update()
    {
        if(currentTilePosition != tilePosition)
        { 
            MoveTile(currentTilePosition, tilePosition);
        }

        if(currentTileColor != tileColor)
        {
            SetTileColor(currentTilePosition, tileColor);
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

        if(tile == null) return;

        tilemap.SetTile(currentPosition, null);
        tilemap.SetTile(newPosition, tile);

        currentTilePosition = newPosition;
    }
}
