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
        public static event Action<Unit> OnDisplayUnitActions;
        public static event Action<Unit, Vector3> OnLeftClickModeListenUnitClick;
        public static event Action<Vector3> OnLeftClickModeListenGroundClick;
        public static event Action OnEscapeKeyForCancelPressed;

        private Camera _mainCamera;
        private LeftClickInputMode leftClickMode;

        private enum LeftClickInputMode {
            LISTEN_UNIT_CLICK,
            LISTEN_GROUND_CLICK,
            LISTEN_UNIT_ACTION_CLICK,
            NONE,
        }

        void Awake()
        {
            GameManager.OnDisplayUnitActionAborded += OnDisplayUnitActionAborded;
            GameManager.OnUnitAttackButtonWasClicked += OnUnitAttackButtonWasClicked;
            GameManager.OnUnitWaitButtonWasClicked += OnUnitWaitButtonWasClicked;
        }

        void OnDestroy()
        {
            GameManager.OnDisplayUnitActionAborded -= OnDisplayUnitActionAborded;
            GameManager.OnUnitAttackButtonWasClicked -= OnUnitAttackButtonWasClicked;
            GameManager.OnUnitWaitButtonWasClicked -= OnUnitWaitButtonWasClicked;
        }


        void Start()
        {
            _mainCamera = Camera.main;
            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
        }

        public void Actualise()
        {
            _mainCamera = Camera.main;
        }

        void OnDisplayUnitActionAborded()
        {
            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
        }

        void OnUnitAttackButtonWasClicked()
        {

        }

        void OnUnitWaitButtonWasClicked()
        {
            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
        }

        void OnCameraSwitched()
        {
            _mainCamera = Camera.main;
        }

        void SetLeftClickMode(LeftClickInputMode mode)
        {
            leftClickMode = mode;
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if(leftClickMode == LeftClickInputMode.LISTEN_GROUND_CLICK || !context.started)
                return;

            RaycastHit hit;
            Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit);

            if(!hit.collider || !hit.collider.gameObject.CompareTag("Unit"))
                return;
            
            Unit unit = hit.collider.gameObject.GetComponent<Unit>();
            
            SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_ACTION_CLICK);

            OnDisplayUnitActions?.Invoke(unit);
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if(!context.started) 
                return;

            RaycastHit hit;
            Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit);

            if(hit.collider)
            {    
                switch (leftClickMode)
                {
                    case LeftClickInputMode.LISTEN_UNIT_CLICK: // display unit moveRange
                        if(!hit.collider.gameObject.CompareTag("Unit")) 
                            return;

                        Unit unit = hit.collider.gameObject.GetComponent<Unit>();

                        Physics.Raycast(hit.collider.transform.position, Vector3.down, out hit);
                        if(!hit.collider || !hit.collider.gameObject.CompareTag("Ground"))
                            return;

                        SetLeftClickMode(LeftClickInputMode.LISTEN_GROUND_CLICK);
                        OnLeftClickModeListenUnitClick?.Invoke(unit, hit.collider.transform.position);
                        break;

                    case LeftClickInputMode.LISTEN_GROUND_CLICK: // Manage unit move
                        if(!hit.collider.gameObject.CompareTag("Ground")) 
                            return;

                        SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
                        OnLeftClickModeListenGroundClick?.Invoke(hit.collider.transform.position);
                        break;

                    default:
                        break;
                }
            }
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if(context.started) 
            {
                SetLeftClickMode(LeftClickInputMode.LISTEN_UNIT_CLICK);
                OnEscapeKeyForCancelPressed?.Invoke();
            }
        }

        public void OnReloadScene(InputAction.CallbackContext context)
        {
            if(context.started) 
            {
                Scene scene = SceneManager.GetActiveScene(); 
                SceneManager.LoadSceneAsync(scene.buildIndex);
            }
        }
    }   
}
