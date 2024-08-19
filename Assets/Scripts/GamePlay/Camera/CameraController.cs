using UnityEngine;

namespace GamePlay.Cameras
{
    public enum CameraMode
    {
        ZOOM_OUT,
        ZOOM_IN,
        MOVE
    }
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float smoothSpeed = 0.125f;

        [Header("Zoom out")]
        [SerializeField] private Quaternion ZoomOutAngle = Quaternion.Euler(62,0,0);
        [SerializeField] private Vector3 ZoomOutOffset = new Vector3 (0, 21, -16);

        private PlayerInputs playerInputs;
        private CameraMode cameraMode;
        private Vector3 zoomTarget;
        private Transform lastCoordinates;

        void Awake()
        {
            playerInputs = new PlayerInputs();
            playerInputs.GamePlay.MoveCamera.Enable(); 

            InputHandler.OnZoomOut += OnZoomOut;
            InputHandler.OnUnZoom += OnUnZoom;
        }

        void Start()
        {
            cameraMode = CameraMode.MOVE;
        }

        void FixedUpdate()
        {
            switch(cameraMode)
            {
                case CameraMode.MOVE:

                    if(lastCoordinates != null && lastCoordinates != transform)
                    {
                        transform.position = SmoothMove(lastCoordinates.position, Vector3.zero);
                        transform.rotation = lastCoordinates.rotation;
                    } 
                    else 
                    {
                        Vector2 inputVector = playerInputs.GamePlay.MoveCamera.ReadValue<Vector2>();
                        if(inputVector != Vector2.zero)
                        {
                            Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);
                            transform.position += movementVector * Time.fixedDeltaTime * speed;
                        }
                    }

                    break;
                case CameraMode.ZOOM_OUT:
                    transform.position = SmoothMove(zoomTarget, ZoomOutOffset);
                    transform.rotation =  ZoomOutAngle;
                    break;

                default:
                    break;

            }
            
        }

        Vector3 SmoothMove(Vector3 target, Vector3 offset)
        {
            Vector3 desiredPosition = target + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            return smoothPosition;
        }

        void OnZoomOut(Vector3 target)
        {
            zoomTarget = target;
            lastCoordinates = transform;
            cameraMode = CameraMode.ZOOM_OUT;
        }

        void OnUnZoom()
        {
            cameraMode = CameraMode.MOVE;
            print("ici");
        }

        void OnDestroy()
        {
            InputHandler.OnZoomOut -= OnZoomOut;
            InputHandler.OnUnZoom-= OnUnZoom;
        }
    }
}
