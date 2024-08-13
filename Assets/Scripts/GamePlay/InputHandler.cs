using UnityEngine;
//using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    
    void Start()
    {
        _mainCamera = Camera.main;
    }

    /*
    public void OnClick(InputAction.CallbackContext context)
    {
        if(!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if(!rayHit.collider) return;

        Warrior warrior = rayHit.collider.gameObject.GetComponent<Warrior>();
        warrior.DrawMovementTiles();
    }
    */
}
