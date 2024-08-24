using Units;
using UnityEngine;

namespace GamePlay.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject camera1;
        [SerializeField] private GameObject camera2;

        public CameraController SwitchCamera(Commander _phase)
        {
            if(_phase == Commander.PLAYER_1)
            {
                camera2.SetActive(false);
                camera1.SetActive(true);

                return camera1.GetComponent<CameraController>();
            }

            camera1.SetActive(false);
            camera2.SetActive(true);

            return camera2.GetComponent<CameraController>();
        }
    }
}
