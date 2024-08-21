using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool VerifieOccupation()
    {
        Vector3 rayStartPosition = new Vector3(transform.position.x, 2f, transform.position.z);
        Vector3 rayDirection = Vector3.down;

        RaycastHit hit;
        Physics.Raycast(rayStartPosition, rayDirection, out hit);

        if(hit.collider.CompareTag("Ground")) return false;
        
        return true;
    }
}
