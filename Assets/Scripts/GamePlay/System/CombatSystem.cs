using System.Collections.Generic;
using UnityEngine;
using Tools;
using GamePlay.Sys;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private PathfindingAStar pathfindingAStar;
    [SerializeField] private TileSystem tileSystem;
    [SerializeField] private GameObject unit;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private int maxRange;
    [SerializeField] private int minRange;



    void Update()
    {
       GetAttackableEnemiesWithoutConsideringObstacles(unit, enemies, maxRange, minRange);
    }

    // Fonction qui retourne les ennemis attaquables sans considérer les obstacles
    public List<Vector3Int> GetAttackableEnemiesWithoutConsideringObstacles(GameObject unit, List<GameObject> enemies, int maxRange, int minRange)
    {
        List<Vector3Int> attackableEnemies = new List<Vector3Int>();

        foreach (GameObject enemy in enemies)
        {
            Vector3Int start = tileSystem.ConvertWorldToCellPosition(unit.transform.position);
            Vector3Int target = tileSystem.ConvertWorldToCellPosition(enemy.transform.position);

            // Utilisation de la fonction FindPath en ignorant les obstacles
            List<Vector3Int> pathToEnemy = pathfindingAStar.FindPath(start, target, false);
            foreach (Vector3Int path in pathToEnemy)
            {
                Debug.Log(path);

            }

            // Si le chemin est vide, cela signifie qu'il n'y a pas de chemin valide
            if (pathToEnemy == null || pathToEnemy.Count == 0)
                continue;
            Debug.Log("Continue");


            // Calcul de la distance du chemin
            int distance = pathToEnemy.Count - 1;

            // Vérification si l'ennemi est dans la portée d'attaque
            if (distance <= maxRange && distance >= minRange)
            {
                Debug.Log("ajout");

                // Ajout des coordonnées de l'ennemi à la liste des ennemis attaquables
                attackableEnemies.Add(target);
            }
        }

        return attackableEnemies;
    }
    public bool NotVerifieWalkability(Vector3 position)
    {
        Vector3 rayStartPosition = new Vector3(position.x, 4f, position.z);
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(rayStartPosition, rayDirection, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Tree"))
            {
                Debug.Log("oui");
                return true;
            }

        }

        return false;
    }
}
