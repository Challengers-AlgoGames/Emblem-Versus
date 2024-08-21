using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        /* Unit attributes */
        [SerializeField] private string title;
        [SerializeField] private float health;
        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int spirituality;
        [SerializeField] private int mobility;
        public int Mobility { get => mobility; }
        [SerializeField] private Inventory[] inventories;
        public Inventory[] Weapons { get => inventories; }

        /* States */
        private const float ATTACK_MULTIPLIER = 1.5f;
        private int currentWeaponIndex = -1;
        [SerializeField] private bool isWasMoved;  // Indicateur d'action de l'unité
        public bool IsWasMoved { get => isWasMoved; }
        private bool isDead;
        [SerializeField] private bool isCanAttack;
        public bool IsCanAttack { get => isCanAttack; }

        /* Movement variables */
        public float moveSpeed = 5f;
        private List<Vector3> movePath;
        public bool IsMoving { get; set; }

        void Awake()
        {
            // Load current weapon index
            for (int i = 0; i < inventories.Length; i++)
            {
                if (inventories[i].isUsed)
                {
                    currentWeaponIndex = i;
                    break;
                }
            }
        }

        void Update()
        {
            // Death state update
            if (health == 0f)
                isDead = true;
        }

        void FixedUpdate()
        {
            // Move
            if (!isWasMoved && movePath != null && movePath.Count > 0 && !IsMoving)
            {
                StartCoroutine(PerformMove());
            }
        }

        public void ResetWasMovedState()
        {
            isWasMoved = false;
        }

        public void Move(List<Vector3> _path)
        {
            movePath = _path;
        }

        IEnumerator PerformMove()
        {
            IsMoving = true;

            foreach (Vector3 point in movePath)
            {
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = new Vector3(point.x, transform.position.y, point.z);

                float journey = 0f;
                float duration = Vector3.Distance(startPosition, targetPosition) / moveSpeed;  // Durée en fonction de la distance

                while (journey <= duration)
                {
                    journey += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, journey / duration);
                    yield return null;
                }
            }

            IsMoving = false;
            isWasMoved = true; // update move state
        }

        public void Wait()
        {
            isWasMoved = true;
        }

        public float CalculateHit()
        {
            float hitRate = 0f;
            float criticalHit = Random.Range(0f, 1f);

            // Accès à l'objet Weapon attaché à l'arme courante
            Weapon usedWeapon = inventories[currentWeaponIndex].item.GetComponent<Weapon>();

            // Calcul du taux de réussite en fonction de la catégorie de l'arme
            WeaponCategory weaponCategory = usedWeapon.Category;
            switch (weaponCategory)
            {
                case WeaponCategory.FIRE_WEAPON:
                    hitRate = usedWeapon.Acuracy;
                    if (criticalHit <= 0.2f)
                        hitRate *= ATTACK_MULTIPLIER;
                    break;

                case WeaponCategory.MELEE_WEAPON:
                    hitRate = usedWeapon.Acuracy + attack;
                    if (criticalHit <= 0.2f)
                        hitRate *= ATTACK_MULTIPLIER;
                    break;

                case WeaponCategory.SPIRITUAL_WEAPON:
                    hitRate = usedWeapon.Acuracy + spirituality;
                    if (criticalHit <= 0.2f)
                        hitRate *= ATTACK_MULTIPLIER;
                    break;

                default:
                    break;
            }

            return hitRate;
        }

        public void Attack(string _usedWeaponName)
        {
            for (int i = 0; i < inventories.Length; i++)
            {
                if (inventories[i].item.gameObject.name.ToLower() == _usedWeaponName.ToLower())
                {
                    inventories[currentWeaponIndex].isUsed = false;

                    inventories[i].isUsed = true;
                    currentWeaponIndex = i;  // Sauvegarde du nouvel index de l'arme utilisée
                }
            }
            isWasMoved = true;
        }

        public void TakeDamages(float _inflictedDamages)
        {
            health = Mathf.Max(health - _inflictedDamages, 0f); // Prend les dégâts
        }
    }
}
