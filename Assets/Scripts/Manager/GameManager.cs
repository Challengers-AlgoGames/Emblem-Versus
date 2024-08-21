using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameStateState;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ChangeState(GameState.GenerateTerrain);
    }

    public void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.GenerateTerrain:
                GridManager.Instance.GenerateTerrain();
                break;
            case GameState.SpawnP1:
                break;
            case GameState.SpawnP2:
                break;
            case GameState.P1turn:
                break;
            case GameState.P2turn:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }
    }
}

public enum GameState
{
    GenerateTerrain = 0,
    SpawnP1 = 1,
    SpawnP2 = 2,
    P1turn = 3,
    P2turn = 4,


}
