using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public static event System.Action OnWasMoved;

        [SerializeField] private string title;
        [SerializeField] private float health;
        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int spirituality;
        [SerializeField] private int mobility;
        [SerializeField] private Weapon weapon;
        [SerializeField] private Commander commander;

        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private bool isWasMoved;
        [SerializeField] private bool isCanMove;
        [SerializeField] private bool isCanAttack;

        private bool isMoving;
        private List<Vector3> movePath;
        private const float ATTACK_MULTIPLIER = 1.5f;

        public bool IsMoving { get => isMoving; }
        public Weapon Weapon { get => weapon; }
        public int Mobility { get => mobility; }
        public bool IsWasMoved { get => isWasMoved; }
        public bool IsCanAttack { get => isCanAttack; }
        public Commander Commander {get => commander; }

        void Start()
        {
            isCanMove = true;
        }

        void FixedUpdate()
        {
            if (!isWasMoved && movePath != null && movePath.Count > 0 && !IsMoving)
            {
                StartCoroutine(PerformMove());
            }
        }

        public void ResetWasMovedState()
        {
            isWasMoved = false;
            isCanMove = true;
        }

        public void Move(List<Vector3> _path)
        {
            if (isWasMoved || !isCanMove)
            {
                Debug.LogWarning("This unit has already moved and cannot move again.");
                return;
            }
            
            movePath = _path;
        }

        IEnumerator PerformMove()
        {
            isMoving = true;

            foreach (Vector3 point in movePath)
            {
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = new Vector3(point.x, transform.position.y, point.z);
                
                float journey = 0f;
                float duration = Vector3.Distance(startPosition, targetPosition) / moveSpeed;

                while (journey <= duration)
                {
                    journey += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, journey / duration);
                    yield return null;
                }
            }

            isMoving = false;
            movePath = null;
            isCanMove = false;
            OnWasMoved?.Invoke();
        }

        public void Wait()
        {
            isWasMoved = true;
            isCanMove = false;
        }

        public float CalculateHit()
        {
            float hitRate = 0f;
            float criticalHit = Random.Range(0f, 1f);

            // Calcul du taux de réussite en fonction de la catégorie de l'arme
            WeaponCategory weaponCategory = weapon.Category;
            switch (weaponCategory)
            {
                case WeaponCategory.FIRE_WEAPON:
                    hitRate = weapon.Acuracy;
                    if (criticalHit <= 0.2f)
                        hitRate *= ATTACK_MULTIPLIER;
                    break;

                case WeaponCategory.MELEE_WEAPON:
                    hitRate = weapon.Acuracy + attack;
                    if (criticalHit <= 0.2f)
                        hitRate *= ATTACK_MULTIPLIER;
                    break;

                case WeaponCategory.SPIRITUAL_WEAPON:
                    hitRate = weapon.Acuracy + spirituality;
                    if (criticalHit <= 0.2f)
                        hitRate *= ATTACK_MULTIPLIER;
                    break;

                default:
                    break;
            }

            return hitRate;
        }

        public void Attack()
        {
            isWasMoved = true;
        }

        public void TakeDamages(float _inflictedDamages)
        {
            health = Mathf.Max(health - _inflictedDamages, 0f); // Prend les dégâts
        }
    }
}
