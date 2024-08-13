using UnityEngine;
using Board;

namespace Units {
public class Unit : MonoBehaviour
{
    public float speed;
    public int maxStep;
    public Commander commander;
    public bool isHaveMove;
    public GameObject tile;
    public void Move()
    {
        if(!isHaveMove && commander == GameBoard.commanderTurn)
        {
            isHaveMove = true;
            Debug.Log(gameObject.name+" was moved");
        }
    }

    public void DrawMovementTiles()
    {
        int maxTile = maxStep;
        Vector3 centerPosition = transform.position;
        Vector3 startingPosition = centerPosition;

        int iteration = maxTile;
        for(int i=0; i < iteration; i++)
        {
            int toGenrarateTile = maxTile - i;
            Vector3 tilePosition;
            for(int j=0; j < toGenrarateTile; j++)
            {
                // y+ && x+
                tilePosition = new Vector3(startingPosition.x + j+1, startingPosition.y + i+1, startingPosition.z + 1);
                Instantiate(tile, tilePosition, Quaternion.identity);

                // y+ && x-
                tilePosition = new Vector3(startingPosition.x - j-1, startingPosition.y + i+1, startingPosition.z + 1);
                Instantiate(tile, tilePosition, Quaternion.identity);

                // // y- && x+
                tilePosition = new Vector3(startingPosition.x + j+1, startingPosition.y - i-1, startingPosition.z + 1);
                Instantiate(tile, tilePosition, Quaternion.identity);

                // // y- && x-
                tilePosition = new Vector3(startingPosition.x - j-1, startingPosition.y - i-1, startingPosition.z + 1);
                Instantiate(tile, tilePosition, Quaternion.identity);
            }
            
        }
    }
}

}
