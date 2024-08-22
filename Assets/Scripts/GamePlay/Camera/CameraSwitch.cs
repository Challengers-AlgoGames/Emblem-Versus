using System;
using UnityEngine;

namespace GamePlay.Cameras
{
    public class CameraSwitch : MonoBehaviour
    {
        public Camera camera1;
        public Camera camera2;
        public bool switchcam;

        void Start()
        {
            // Activer la première caméra et désactiver la seconde au démarrage
            camera1.enabled = true;
            camera2.enabled = false;
        }

        void Update()
        {
            // Tester la fonction SwitchCameras() lorsque la variable switchcam est true
            if (switchcam)
            {
                SwitchCameras();
                switchcam = false; // Réinitialiser la variable pour éviter de switcher continuellement
            }
        }

        // Fonction pour switcher entre les deux caméras
        public void SwitchCameras()
        {
            camera1.enabled = !camera1.enabled;
            camera2.enabled = !camera2.enabled;
        }
    }
}
