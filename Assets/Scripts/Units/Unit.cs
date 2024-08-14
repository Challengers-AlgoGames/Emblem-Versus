using System;
using UnityEngine;
using Weapons;

public class Unit : MonoBehaviour
{
    /* Units infos */
    [SerializeField] private string title;
    [SerializeField] private float health;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int spitituality;
    [SerializeField] private int mobility; 

    /* inventory */
    [Serializable]
    public struct Inventory
    {
        public GameObject item;
        public bool isUsed;
    }
    [SerializeField] private Inventory[] inventories;

    /* states */
    private const float ATTACK_MULTIPLIER = 1.5f;
    private int currendWeaponIndex = -1;
    private bool isHaveMoved; // unit actions controller
    private bool isDead; 
    private bool isCanAttack;

    void Awake()
    {
        // save unity used wapon index for last use
        for(int i = 0; i < inventories.Length; i++) {
            if(inventories[i].isUsed)
            {
                currendWeaponIndex = i;
                break;
            }
        }
    }

    void Update()
    {
        // active used weapon to hiararchy
        GameObject currentWeapon = inventories[currendWeaponIndex].item;
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

    public float Attack() 
    {
        float hitRate = 0f;
        float criticalHit = UnityEngine.Random.Range(0f, 1f);

        // access current weapon onject attached Weapon script
        Weapon usedWeapon = inventories[currendWeaponIndex].item.GetComponent<Weapon>();

        // Apply apropriate hit calulation
        WeaponCategory weaponCategory = usedWeapon.Category;
        switch(weaponCategory)
        {
            case WeaponCategory.FIRE_WEAPON:
                hitRate = usedWeapon.Acuracy;
                if(criticalHit <= 0.2f)
                    hitRate *= ATTACK_MULTIPLIER;
                break;

            case WeaponCategory.MELEE_WEAPON:
                hitRate = usedWeapon.Acuracy + attack;
                
                if(criticalHit <= 0.2f)
                    hitRate *= ATTACK_MULTIPLIER;
                break;

            case WeaponCategory.SPIRITUAL_WEAPON:
                hitRate = usedWeapon.Acuracy + spitituality;
                if(criticalHit <= 0.2f)
                    hitRate *= ATTACK_MULTIPLIER;
                break;

            default:
                break;
        }

        return hitRate;
    }

    public void SwitchWeapon(int _index)
    {
        inventories[currendWeaponIndex].item.SetActive(false); // deactive last weapon to hiararchy
        currendWeaponIndex =  _index; // save new used weapon index
        isHaveMoved = true;
    }

    public void TakeDamages(float _inflictedDamages)
    {
        health = Mathf.Max(health - _inflictedDamages, 0f); // take damages
    }
}
