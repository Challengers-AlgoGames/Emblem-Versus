using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // Méthode pour lancer un raycast à une position spécifique et obtenir le GameObject touché
    public GameObject GetObjectAtPosition(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }

        return null; // Retourne null si aucun objet n'est touché
    }

    // Méthode pour vérifier si un GameObject a le tag "Ground"
    public bool IsWalkable(GameObject tile) => tile.CompareTag("Ground");
}
