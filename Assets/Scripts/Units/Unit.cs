using UnityEngine;
using Weapons;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        /* Units infos */
        [SerializeField] private string title;
        [SerializeField] private float health;
        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int spirituality;
        [SerializeField] private int mobility;
        public int Mobility { get => mobility; }
        [SerializeField] private Inventory[] inventories;
        public Inventory[] Weapons { get => inventories; }

        /* states */
        private const float ATTACK_MULTIPLIER = 1.5f;
        private int currentWeaponIndex = -1;
        [SerializeField] private bool isWasMoved; // unit actions controller
        public bool IsWasMoved { get => isWasMoved; }
        private bool isDead;
        [SerializeField] private bool isCanAttack;
        public bool IsCanAttack { get => isCanAttack; }
        public float moveSpeed = 5f; // Vitesse de déplacement
        private Vector3 targetPosition; // Position cible de l'unité
        public bool IsMoving { get; set; }// Indicateur pour savoir si l'unité est en mouvement

        void Awake()
        {
            // Save the index of the used weapon for the last use
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
            // Change unit state to dead
            if (health <= 0f)
            {
                isDead = true;
                IsMoving = false;
                return;
            }

            if (IsMoving)
            {
                // Move the unit towards the target position
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                // Stop movement if the unit has reached the target position
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    transform.position = targetPosition;
                    IsMoving = false;
                }
            }
        }

        public void ResetWasMovedState()
        {
            isWasMoved = false;
        }

        public void Move(Vector3 _target)
        {
            // Create a new position while keeping the current height
            targetPosition = new Vector3(_target.x, transform.position.y, _target.z);
            IsMoving = true;
        }

        public void Wait()
        {
            isWasMoved = true;
            IsMoving = false;
        }

        public float CalculateHit()
        {
            float hitRate = 0f;
            float criticalHit = Random.Range(0f, 1f);

            // Access the current weapon object and its Weapon script
            if (currentWeaponIndex < 0 || currentWeaponIndex >= inventories.Length)
            {
                Debug.LogError("Current weapon index is out of bounds.");
                return hitRate;
            }

            Weapon usedWeapon = inventories[currentWeaponIndex].item.GetComponent<Weapon>();

            if (usedWeapon == null)
            {
                Debug.LogError("Weapon component not found on the current weapon.");
                return hitRate;
            }

            // Apply appropriate hit calculation
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
                    Debug.LogWarning("Unknown weapon category.");
                    break;
            }

            return hitRate;
        }

        public void Attack(string _usedWeaponName)
        {
            for (int i = 0; i < inventories.Length; i++)
            {
                if (inventories[i].item.gameObject.name.Equals(_usedWeaponName, System.StringComparison.OrdinalIgnoreCase))
                {
                    if (currentWeaponIndex >= 0 && currentWeaponIndex < inventories.Length)
                    {
                        inventories[currentWeaponIndex].isUsed = false;
                    }

                    inventories[i].isUsed = true;
                    currentWeaponIndex = i; // Save new used weapon index
                    isWasMoved = true;
                    return;
                }
            }

            Debug.LogWarning("Weapon not found: " + _usedWeaponName);
        }

        public void TakeDamages(float _inflictedDamages)
        {
            health = Mathf.Max(health - _inflictedDamages, 0f); // Apply damage
        }
    }
}
