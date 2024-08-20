using UnityEngine;

namespace GamePlay.Cameras
{
    public enum CameraMode
    {
        ZOOM_OUT,
        ZOOM_IN,
        UNZOOM,
        MOVE
    }
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float smoothSpeed = 0.125f;

        [Header("Zoom out")]
        [SerializeField] private Quaternion zoomOutAngle = Quaternion.Euler(62,0,0);
        [SerializeField] private Vector3 zoomOutOffset = new Vector3 (0, 21, -16);

        private PlayerInputs playerInputs;
        private CameraMode cameraMode;
        private Vector3 zoomTarget;
        private Vector3 positionBeforZoom;
        private Quaternion rotationBeforZoom;

        void Awake()
        {
            playerInputs = new PlayerInputs();
            playerInputs.GamePlay.MoveCamera.Enable(); 

            MoveSystem.OnZoomOut += OnZoomOut;
            MoveSystem.OnUnZoom += OnUnZoom;
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
                    Vector2 inputVector = playerInputs.GamePlay.MoveCamera.ReadValue<Vector2>();
                    if(inputVector != Vector2.zero)
                    {
                        Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);
                        transform.position += movementVector * Time.fixedDeltaTime * speed;
                    }
                    break;

                case CameraMode.ZOOM_OUT:
                    transform.position = SmoothMove(zoomTarget, zoomOutOffset);
                    transform.rotation = SmoothRotation(zoomOutAngle);
                    // Check if we have reached the zoomed out position
                    if (Vector3.Distance(transform.position, zoomTarget + zoomOutOffset) < 0.01f)
                    {
                        cameraMode = CameraMode.MOVE;
                    }
                    break;

                case CameraMode.UNZOOM:
                    positionBeforZoom.x = transform.position.x;
                    transform.position = SmoothMove(positionBeforZoom, Vector3.zero);
                    transform.rotation = SmoothRotation(rotationBeforZoom);
                    // Check if we have returned to the original position
                    if (Vector3.Distance(transform.position, positionBeforZoom) < 0.01f &&
                        Quaternion.Angle(transform.rotation, rotationBeforZoom) < 1f)
                    {
                        cameraMode = CameraMode.MOVE;
                    }
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

        Quaternion SmoothRotation(Quaternion target)
        {
            return Quaternion.Lerp(transform.rotation, target, smoothSpeed);
        }

        void OnZoomOut(Vector3 target)
        {
            zoomTarget = target;
            positionBeforZoom = transform.position;
            rotationBeforZoom = transform.rotation;
            cameraMode = CameraMode.ZOOM_OUT;
        }

        void OnUnZoom()
        {
            cameraMode = CameraMode.UNZOOM;
        }

        void OnDestroy()
        {
            MoveSystem.OnZoomOut -= OnZoomOut;
            MoveSystem.OnUnZoom-= OnUnZoom;
        }
    }
}
