using UnityEngine;

public class Ground : MonoBehaviour
{
    public static bool VerifieWalkability(Vector3 position)
    {
        Vector3 rayStartPosition = new Vector3(position.x, 4f, position.z);
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(rayStartPosition, rayDirection, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Ground")) return true;
        }

        return false;
    }
}
