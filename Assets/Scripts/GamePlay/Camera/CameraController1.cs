// using System;
// using UnityEngine;

// namespace GamePlay.Cameras
// {
//     public enum CameraMode
//     {
//         ZOOM_OUT,
//         ZOOM_IN,
//         UNZOOM,
//         MOVE,
//         NONE,
//     }
    
//     public class CameraController : MonoBehaviour
//     {
//         public static event Action OnZoomOut;
//         public static event Action OnUnZoom;

//         [SerializeField] private float speed = 5f;
//         [SerializeField] private float smoothSpeed = 0.125f;

//         [Header("Zoom out")]
//         [SerializeField] private Quaternion zoomOutAngle = Quaternion.Euler(62, 0, 0);
//         [SerializeField] private Vector3 zoomOutOffset = new Vector3(0, 21, -16);

//         private PlayerInputs playerInputs;
//         private CameraMode cameraMode = CameraMode.NONE;
//         private Vector3 zoomTarget;
//         private Vector3 positionBeforZoom;
//         private Quaternion rotationBeforZoom;

//         void Awake()
//         {
//             playerInputs = new PlayerInputs();
//             playerInputs.GamePlay.MoveCamera.Enable();

//             GameManager.OnGameStated += OnGameStated;
//             MoveSystem.OnDisplayMoveRange += ZoomOut;
//             MoveSystem.OnClearActiveTile += UnZoom;
//         }

//         void OnDestroy()
//         {
//             GameManager.OnGameStated -= OnGameStated;
//             MoveSystem.OnDisplayMoveRange -= ZoomOut;
//             MoveSystem.OnClearActiveTile -= UnZoom;
//         }

//         public void OnGameStated()
//         {
//             cameraMode = CameraMode.MOVE;
//             rotationBeforZoom = transform.rotation;
//         }

//         void FixedUpdate()
//         {
//             if (cameraMode == CameraMode.NONE) return;

//             switch (cameraMode)
//             {
//                 case CameraMode.MOVE:
//                     HandleCameraMovement();
//                     break;

//                 case CameraMode.ZOOM_OUT:
//                     HandleZoomOut();
//                     break;

//                 case CameraMode.UNZOOM:
//                     HandleUnZoom();
//                     break;

//                 default:
//                     break;
//             }
//         }

//         void HandleCameraMovement()
//         {
//             Vector2 inputVector = playerInputs.GamePlay.MoveCamera.ReadValue<Vector2>();
//             if (inputVector != Vector2.zero)
//             {
//                 Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);
//                 transform.position += movementVector * Time.fixedDeltaTime * speed;
//             }
//         }

//         void HandleZoomOut()
//         {
//             transform.position = SmoothMove(zoomTarget, zoomOutOffset);
//             transform.rotation = SmoothRotation(zoomOutAngle);
//             if (HasReachedTarget(transform.position, zoomTarget + zoomOutOffset) &&
//                 HasReachedRotation(transform.rotation, zoomOutAngle))
//             {
//                 cameraMode = CameraMode.MOVE;
//             }
//         }

//         void HandleUnZoom()
//         {
//             transform.position = SmoothMove(positionBeforZoom, Vector3.zero);
//             transform.rotation = SmoothRotation(rotationBeforZoom);
//             if (HasReachedTarget(transform.position, positionBeforZoom) &&
//                 HasReachedRotation(transform.rotation, rotationBeforZoom))
//             {
//                 cameraMode = CameraMode.MOVE;
//             }
//         }

//         Vector3 SmoothMove(Vector3 target, Vector3 offset)
//         {
//             Vector3 desiredPosition = target + offset;
//             return Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//         }

//         Quaternion SmoothRotation(Quaternion target)
//         {
//             return Quaternion.Lerp(transform.rotation, target, smoothSpeed);
//         }

//         bool HasReachedTarget(Vector3 current, Vector3 target)
//         {
//             return Vector3.Distance(current, target) < 0.01f;
//         }

//         bool HasReachedRotation(Quaternion current, Quaternion target)
//         {
//             return Quaternion.Angle(current, target) < 1f;
//         }

//         void ZoomOut(Vector3 target)
//         {
//             zoomTarget = target;
//             positionBeforZoom = transform.position;
//             rotationBeforZoom = transform.rotation; // Correction pour capturer la rotation avant le zoom
//             cameraMode = CameraMode.ZOOM_OUT;
//             OnZoomOut?.Invoke();
//         }

//         void UnZoom()
//         {
//             cameraMode = CameraMode.UNZOOM;
//             OnUnZoom?.Invoke();
//         }
//     }
// }
