using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;

    void Awake()
    {
        Instance = this;
    }
}
