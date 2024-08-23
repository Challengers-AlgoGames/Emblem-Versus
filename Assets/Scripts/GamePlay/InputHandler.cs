using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Units;
using UnityEngine.SceneManagement;
using GamePlay.Cameras;

namespace GamePlay
{

    public class InputHandler : MonoBehaviour
    {
        public static event Action OnReady;

        /* unit events */
        public static event Action<Unit> OnDisplayUnitActions;
        public static event Action<Unit, Vector3> OnLeftClickModeListenUnitClick;
        public static event Action<Vector3> OnLeftClickModeListenGroundClick;
        public static event Action OnEscapeKeyForCancelPressed;

        private Camera _mainCamera;
        private LeftClickInputMode leftClickMode;

        private enum LeftClickInputMode {
            LISTEN_UNIT_CLICK,
            LISTEN_GROUND_CLICK,
            NONE
        }

        void Awake()
        {
            CameraSwitch.OnCameraSwitched += OnCameraSwitched;
        }
        
        void OnDestroy()
        {
            CameraSwitch.OnCameraSwitched -= OnCameraSwitched;
        }

        void Start()
        {
            _mainCamera = Camera.main;
            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
            OnReady?.Invoke();
        }

        void OnCameraSwitched()
        {
            _mainCamera = Camera.main;
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
            
            Unit unit = hit.collider.gameObject.GetComponent<Unit>();

            OnDisplayUnitActions?.Invoke(unit);
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

                    Physics.Raycast(hit.collider.transform.position, Vector3.down, out hit);
                    if(!hit.collider)
                    {
                        return;
                    }
                    if(!hit.collider.gameObject.CompareTag("Ground"))
                    {
                        return;
                    }

                    OnLeftClickModeListenUnitClick?.Invoke(unit, hit.collider.transform.position);
                    SetLeftClickMode(LeftClickInputMode.LISTEN_GROUND_CLICK);

                    break;
                case LeftClickInputMode.LISTEN_GROUND_CLICK: // Manage unit move
                    if(!hit.collider.gameObject.CompareTag("Ground")) return;

                    OnLeftClickModeListenGroundClick?.Invoke(hit.collider.transform.position);
                    
                    SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);

                    break;
                default:
                    print("Choise not supported");
                    break;
            }
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if(!context.started) 
            {
                return;
            }

            switch (leftClickMode)
            {
                case LeftClickInputMode.LISTEN_GROUND_CLICK:
                    SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
                    OnEscapeKeyForCancelPressed?.Invoke();
                    break;
                case LeftClickInputMode.LISTEN_UNIT_CLICK:
                    print("Pause");
                    break;
                default:
                    print("Choise not supported");
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
