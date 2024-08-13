using UnityEngine;
using System.Collections.Generic;
using Units;

namespace Board
{
public class GameBoard : MonoBehaviour
{
    public Army commander1_army;
    public Army commander2_army;
    
    public static Commander commanderTurn;
    private int turnNumber;
    private Dictionary<Commander, bool> ARMIES_WAS_MOVED = new()
    {
        {Commander.PLAYER_1, false},
        {Commander.PLAYER_2, false}
    };

    void Awake()
    {
        commander1_army.OnArmyWasMoved += OnCommander1_ArmyWasMove;
        commander2_army.OnArmyWasMoved += OnCommander2_ArmyWasMove;
    }

    void Start()
    {
        turnNumber = 1;
        commanderTurn = Commander.PLAYER_1;

        Debug.Log("Turn : " + turnNumber);
        Debug.Log("Commander 1 turn");
    }

    void Update()
    {
        if(ARMIES_WAS_MOVED[Commander.PLAYER_1] && ARMIES_WAS_MOVED[Commander.PLAYER_2])
        {
            turnNumber++;

            ARMIES_WAS_MOVED[Commander.PLAYER_1] = false;
            ARMIES_WAS_MOVED[Commander.PLAYER_2] = false;

            Debug.Log("Turn : " + turnNumber);
            Debug.Log("Commander 1 turn");
        }
    }

    void OnCommander1_ArmyWasMove()
    {
        ARMIES_WAS_MOVED[Commander.PLAYER_1] = true;
        commanderTurn = Commander.PLAYER_2;
        Debug.Log("Commander 2 turn");
    }

    void OnCommander2_ArmyWasMove()
    {
        ARMIES_WAS_MOVED[Commander.PLAYER_2] = true;
        commanderTurn = Commander.PLAYER_1;
        Debug.Log("Commander 1 turn");
    }

    void OnDestroy()
    {
        commander1_army.OnArmyWasMoved -= OnCommander1_ArmyWasMove;
        commander2_army.OnArmyWasMoved -= OnCommander2_ArmyWasMove;
    }
}
}
