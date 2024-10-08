using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Units;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public enum LeftClickInputMode
    {
        LISTEN_UNIT_CLICK,
        LISTEN_GROUND_CLICK,
        LISTEN_UNIT_ACTION_CLICK,
        LISTEN_ATK_TARGET_CLICK,
        NONE,
    }

    public class InputHandler : MonoBehaviour
    {
        public static event Action<Unit> OnDisplayUnitActions;
        public static event Action OnEscapeKeyForCancelPressed;
        // public static event Action<Vector3> OnLeftClickModeListenAtkClick;
        public static event Action<Vector3> OnLeftClickModeListenGroundClick;
        public static event Action<Unit> OnLeftClickModeListenUnitActionClick;
        public static event Action<Unit, Vector3> OnLeftClickModeListenUnitClick;
        
        public bool StopListenInput {get; set;}

        private Camera _mainCamera;
        private LeftClickInputMode leftClickMode;

        void Start()
        {
            _mainCamera = Camera.main;
            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
        }

        public void Actualise()
        {
            _mainCamera = Camera.main;
        }

        public void LeftClickListen(LeftClickInputMode _mode)
        {
            SetLeftClickMode(_mode);
        }

        void SetLeftClickMode(LeftClickInputMode mode)
        {
            leftClickMode = mode;
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (leftClickMode != LeftClickInputMode.LISTEN_UNIT_CLICK || !context.started)
                return;

            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
            {
                if (hit.collider && hit.collider.gameObject.CompareTag("Unit"))
                {
                    Unit unit = hit.collider.gameObject.GetComponent<Unit>();
                    SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_ACTION_CLICK);
                    OnDisplayUnitActions?.Invoke(unit);
                }
            }
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if (!context.started)
                return;

            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
            {
                switch (leftClickMode)
                {
                    case LeftClickInputMode.LISTEN_UNIT_CLICK:
                        HandleUnitClick(hit);
                        break;

                    case LeftClickInputMode.LISTEN_GROUND_CLICK:
                        HandleGroundClick(hit);
                        break;

                    case LeftClickInputMode.LISTEN_UNIT_ACTION_CLICK:
                        HandleUnitActionClick(hit);
                        break;

                    default:
                        break;
                }
            }
        }

        void HandleUnitClick(RaycastHit hit)
        {
            if (!hit.collider.gameObject.CompareTag("Unit"))
                return;

            Unit unit = hit.collider.gameObject.GetComponent<Unit>();

            if (Physics.Raycast(hit.collider.transform.position, Vector3.down, out RaycastHit groundHit) &&
                groundHit.collider && groundHit.collider.gameObject.CompareTag("Ground"))
            {
                SetLeftClickMode(LeftClickInputMode.LISTEN_GROUND_CLICK);
                OnLeftClickModeListenUnitClick?.Invoke(unit, groundHit.collider.transform.position);
            }
        }

        void HandleGroundClick(RaycastHit hit)
        {
            if (!hit.collider.gameObject.CompareTag("Ground"))
                return;

            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
            OnLeftClickModeListenGroundClick?.Invoke(hit.collider.transform.position);
        }

        void HandleUnitActionClick(RaycastHit hit)
        {
            if(hit.collider && hit.collider.CompareTag("Unit"))
            {
                OnLeftClickModeListenUnitActionClick?.Invoke(hit.collider.GetComponent<Unit>());
            }
            
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if (context.started && !StopListenInput)
            {
                SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
                OnEscapeKeyForCancelPressed?.Invoke();
            }
        }

        public void OnReloadScene(InputAction.CallbackContext context)
        {
            if (context.started && StopListenInput)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadSceneAsync(scene.buildIndex);
            }
        }
    }
}
