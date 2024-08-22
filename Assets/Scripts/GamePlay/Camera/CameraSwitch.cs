using System;
using Units;
using UnityEngine;

namespace GamePlay.Cameras
{
    public class CameraSwitch : MonoBehaviour
    {
        public static event Action OnCameraSwitched;

        [SerializeField] private GameObject camera1;
        [SerializeField] private GameObject camera2;

        void Awake()
        {
            TurnBaseSystem.OnPhaseUpdate += OnPhaseUpdate;
        }

        void OnDestroy()
        {
            TurnBaseSystem.OnPhaseUpdate -= OnPhaseUpdate;
        }

        void OnPhaseUpdate(Commander _phase)
        {
            switch (_phase)
            {
                case Commander.PLAYER_1:
                    camera1.SetActive(true);
                    camera2.SetActive(false);
                    break;
                case Commander.PLAYER_2:
                    camera1.SetActive(false);
                    camera2.SetActive(true);
                    break;
                default:
                    Debug.Log("Not supported");
                    break;
            }

            OnCameraSwitched?.Invoke();
        }
    }
}
