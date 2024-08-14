using UnityEngine;
using System.Collections.Generic;
using Units;

namespace GamePlay
{
public class Board : MonoBehaviour
{
    /* Board states */
    public static Commander commanderTurn;
    private int turnNumber;
    private Dictionary<Commander, bool> ARMIES_WAS_MOVED = new()
    {
        {Commander.PLAYER_1, false},
        {Commander.PLAYER_2, false}
    };

    /* Players */
    [SerializeField] private Army commander1_army;
    [SerializeField] private Army commander2_army;

    void Awake()
    {
        /* Listen Army class events */
        commander1_army.OnArmyWasMoved += OnCommander1_ArmyWasMove;
        commander2_army.OnArmyWasMoved += OnCommander2_ArmyWasMove;

        /* Set biginer player */
        commanderTurn = Commander.PLAYER_1;
    }

    void Start()
    {
        turnNumber = 1; // initialise turn number

        Debug.Log("Turn : " + turnNumber);
        Debug.Log("Commander 1 turn");
    }

    void Update()
    {
        /* Pass to next turn */
        if(ARMIES_WAS_MOVED[Commander.PLAYER_1] && ARMIES_WAS_MOVED[Commander.PLAYER_2])
        {
            turnNumber++;

            /* Reset Armies move states */
            ARMIES_WAS_MOVED[Commander.PLAYER_1] = false;
            ARMIES_WAS_MOVED[Commander.PLAYER_2] = false;

            Debug.Log("Turn : " + turnNumber);
            Debug.Log("Commander 1 turn");
        }
    }

   /* Turn base manager */
    void OnCommander1_ArmyWasMove()
    {
        ARMIES_WAS_MOVED[Commander.PLAYER_1] = true;
        commanderTurn = Commander.PLAYER_2; // Past turn to the second player
        Debug.Log("Commander 2 turn");
    }
    void OnCommander2_ArmyWasMove()
    {
        ARMIES_WAS_MOVED[Commander.PLAYER_2] = true;
        commanderTurn = Commander.PLAYER_1; // Past turn to the second player
        Debug.Log("Commander 1 turn");
    }

    /* Unlistent Army class events if destroied */
    void OnDestroy()
    {
        commander1_army.OnArmyWasMoved -= OnCommander1_ArmyWasMove;
        commander2_army.OnArmyWasMoved -= OnCommander2_ArmyWasMove;
    }
}
}
