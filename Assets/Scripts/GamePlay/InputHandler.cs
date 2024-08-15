using UnityEngine;
using UnityEngine.InputSystem;
using UnitUI;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;
    
    void Start()
    {
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if(!context.started) return;

        /* Detection */
        RaycastHit hit;
        Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit);

        if(!hit.collider) return; // the hit haven't collider

        // check if hit is a unit
        if(hit.collider.gameObject.CompareTag("Unit"))
        {
            UnitActionsUIController unitActionsUIController = hit.collider.gameObject.GetComponent<UnitActionsUIController>();
            unitActionsUIController.ShowUnitPossibleActionsUI();
        }
    }
}
