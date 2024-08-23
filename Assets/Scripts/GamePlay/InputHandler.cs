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
        /* unit events */
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

        public void Active()
        {
            Actualise();
            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
        }

        public void Actualise()
        {
            Debug.Log("actualise");
            _mainCamera = Camera.main;
        }

        void SetLeftClickMode(LeftClickInputMode mode)
        {
            leftClickMode = mode;
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            
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
                case LeftClickInputMode.LISTEN_UNIT_CLICK:
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
                case LeftClickInputMode.LISTEN_GROUND_CLICK:
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
