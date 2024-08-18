using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Units;
using UnityEngine.SceneManagement;

namespace GamePlay
{

    public class InputHandler : MonoBehaviour
    {
        public static event Action OnDisplayUnitActions;
        public static event Action<Unit, Vector3> OnDisplayUnitMoveRange;
        public static event Action<Vector3> OnMoveUnit;
        public static event Action OnResetUnitMove;
        public static event Action OnClearUI;

        private Camera _mainCamera;
        private LeftClickInputMode leftClickMode;
        private EscapeKeyMode escapeKeyMode;

        private enum LeftClickInputMode {
            LISTEN_UNIT_CLICK,
            LISTEN_GROUND_CLICK,
            NONE
        }
        
        private enum EscapeKeyMode {
            CANCEL,
            CLOSE
        }

        void Start()
        {
            _mainCamera = Camera.main;

            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
        }

        void SetEscapeKeyMode(EscapeKeyMode mode)
        {
            escapeKeyMode = mode;
        }

        void SetLeftClickMode(LeftClickInputMode mode)
        {
            leftClickMode = mode;
        }

        // Display unit actions
        public void OnRightClick(InputAction.CallbackContext context)
        {
            if(leftClickMode == LeftClickInputMode.LISTEN_GROUND_CLICK)
            {
                return;
            }

            if(!context.started) 
            {
                return;
            }

            /* Detection */
            RaycastHit hit;
            Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit);

            if(!hit.collider)
            {
                return; // the hit haven't collider
            }

            // check if hit is a unit   
            if(!hit.collider.gameObject.CompareTag("Unit"))
            {
                return;
            }
            
            OnDisplayUnitActions?.Invoke();  
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if(!context.started) 
            {
                return;
            }

            RaycastHit hit;
            Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit);
            
            if(!hit.collider)
            {
                return;
            }
            
            switch (leftClickMode)
            {
                case LeftClickInputMode.LISTEN_UNIT_CLICK: // display unit moveRange
                    if(!hit.collider.gameObject.CompareTag("Unit")) return;
                    
                    Unit unit = hit.collider.gameObject.GetComponent<Unit>();
                    
                    //Take Ground tile gameobjet
                    Physics.Raycast(hit.collider.transform.position, Vector3.down, out hit);
                    if(!hit.collider)
                    {
                        return;
                    }
                    if(!hit.collider.gameObject.CompareTag("Ground"))
                    {
                        return;
                    }

                    OnDisplayUnitMoveRange?.Invoke(unit, hit.collider.transform.position);

                    SetLeftClickMode(LeftClickInputMode.LISTEN_GROUND_CLICK);
                    SetEscapeKeyMode(EscapeKeyMode.CLOSE);
                
                    break;
                case LeftClickInputMode.LISTEN_GROUND_CLICK: // Manage unit move
                    if(!hit.collider.gameObject.CompareTag("Ground")) return;
                    OnMoveUnit?.Invoke(hit.collider.transform.position);

                    SetLeftClickMode(LeftClickInputMode.NONE);
                    SetEscapeKeyMode(EscapeKeyMode.CANCEL);
                    
                    break;
                default:
                    break;
            }
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if(!context.started) 
            {
                return;
            }

            switch(escapeKeyMode)
            {
                case EscapeKeyMode.CANCEL:
                    OnResetUnitMove?.Invoke();
                    SetLeftClickMode(LeftClickInputMode.LISTEN_GROUND_CLICK);
                    SetEscapeKeyMode(EscapeKeyMode.CLOSE);
                    break;
                case EscapeKeyMode.CLOSE:
                    OnClearUI?.Invoke();
                    SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
                    break;
                default:
                    break;
            }
            
        }

        public void OnReloadScene(InputAction.CallbackContext context)
        {
            if(!context.started) 
            {
                return;
            }
            
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadSceneAsync(scene.buildIndex);
        }
    }   
}
