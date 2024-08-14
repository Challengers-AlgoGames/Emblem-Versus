using System;
using UnityEngine;
using Units;

public class Army : MonoBehaviour
{
    public event Action OnArmyWasMoved;

    public Unit[] units;


    // void Update()
    // {
    //     bool armyWasMove = VerifyArmyWasMove();
    //     if(armyWasMove)
    //     {
    //         OnArmyWasMoved?.Invoke();
    //         ResetArmyMoveState();
    //     }
    // }

    // bool VerifyArmyWasMove()
    // {
    //     foreach (var unit in units)
    //     {
    //         if(unit.isHaveMove == false) return false;
    //     }
    //     return true;
    // }

    // void ResetArmyMoveState()
    // {
    //     foreach (var unit in units)
    //     {
    //         unit.isHaveMove = false;
    //     }
    // }
}
