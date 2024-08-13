using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /* Units states */
    [SerializeField] private string title;
    [SerializeField] private float health;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int spitituality;
    [SerializeField] private int mobility; 

    /* Weapon inventory */
    [Serializable]
    public struct WeaponStruct
    {
        public GameObject weapon;
        public bool isUsed;
    }
    [SerializeField] private WeaponStruct[] weapons;
    public WeaponStruct[] WeaponList { get=> weapons; }

    /* variables need to do unit class work */
    private bool isHaveMove = false;
    private int currendWeaponIndex = -1;

    void Start()
    {
        // save unity used wapon index for last use
        for(int i = 0; i < weapons.Length; i++) {
            if(weapons[i].isUsed)
            {
                currendWeaponIndex = i;
                break;
            }
        }
    }

    void Update()
    {
        // active used weapon to hiararchy
        GameObject currentWeapon = weapons[currendWeaponIndex].weapon;
        if(!currentWeapon.activeInHierarchy)
            currentWeapon.SetActive(true);
    }

    public void SwitchWeapon(int _index)
    {
        weapons[currendWeaponIndex].weapon.SetActive(false); // deactive last weapon to hiararchy
        currendWeaponIndex =  _index; // save new used weapon index
    }
}
