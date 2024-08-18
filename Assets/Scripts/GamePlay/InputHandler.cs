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

        private Camera _mainCamera;
        
        void Start()
        {
            _mainCamera = Camera.main;
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
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
            
            if(!hit.collider.gameObject.CompareTag("Unit"))
            {
                return;
            }
            
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

            Vector3 cellulWorldPosition = hit.collider.transform.position;

            OnDisplayUnitMoveRange?.Invoke(unit, cellulWorldPosition);
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
