using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /* Units infos */
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

    /* states */
    private int currendWeaponIndex = -1;
    private bool isHaveMoved; // unit actions controller
    private bool isDead; 

    void Awake()
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

    void Start()
    {
        
    }

    void Update()
    {
        // active used weapon to hiararchy
        GameObject currentWeapon = weapons[currendWeaponIndex].weapon;
        if(!currentWeapon.activeInHierarchy)
            currentWeapon.SetActive(true);
        
        // change unit state to dead
        if(health == 0f)
            isDead = true;
    }

    public void Move() {}

    public void DoNothing() 
    {
        isHaveMoved = true;
    }

    public int Attack() 
    {
        GameObject currentWeapon = weapons[currendWeaponIndex].weapon;
        isHaveMoved = true;
        return attack;
    }

    public void SwitchWeapon(int _index)
    {
        weapons[currendWeaponIndex].weapon.SetActive(false); // deactive last weapon to hiararchy
        currendWeaponIndex =  _index; // save new used weapon index
        isHaveMoved = true;
    }

    public void TakeDamages(float _inflictedDamages)
    {
        health = Mathf.Max(health - _inflictedDamages, 0f); // take damages
    }
}
