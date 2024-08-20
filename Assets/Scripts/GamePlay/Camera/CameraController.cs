using UnityEngine;

namespace GamePlay.Cameras
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        private PlayerInputs playerInputs;
        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;

            playerInputs = new PlayerInputs();
            playerInputs.GamePlay.MoveCamera.Enable(); 
        }

        void FixedUpdate()
        {
            Vector2 inputVector = playerInputs.GamePlay.MoveCamera.ReadValue<Vector2>();

            if(inputVector != Vector2.zero)
            {
                Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);
                rb.MovePosition(rb.position + movementVector * speed * Time.fixedDeltaTime);
            }
        }
    }
}
