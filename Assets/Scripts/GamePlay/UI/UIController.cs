using GamePlay.Cameras;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UIs
{
    /* Dependences : CameraController */
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Transform topLeftContainer;
        [SerializeField] private Transform bottomContainer;

        private Text topLeftHelpText;
        private Text bottomLeftHelpText;
        private Text bottomRighttHelpText;

        void Awake()
        {
           CameraController.OnZoomOut += OnZoomOut;
           CameraController.OnUnZoom += OnUnZoom;
        }

        void OnDestroy()
        {
            CameraController.OnZoomOut -= OnZoomOut;
            CameraController.OnUnZoom -= OnUnZoom;
        }

        void Start()
        {
            GameObject leftHelpObj = Instantiate(textPrefab, topLeftContainer);
            topLeftHelpText = leftHelpObj.GetComponent<Text>();
            SetTopLeftText("(Esc) Menu");

            DisplayBottomHelp();
        }

        void OnZoomOut()
        {
            SetTopLeftText("(Esc) Cancel");
            DestroyBottomHelp();
        }

        void OnUnZoom()
        {
            SetTopLeftText("(Esc) Menu");
            DisplayBottomHelp();
        }

        void SetTopLeftText(string text)
        {
            topLeftHelpText.text = text;
        }

        void DisplayBottomHelp()
        {
            GameObject leftBottomHelpObj = Instantiate(textPrefab, bottomContainer);
            bottomLeftHelpText = leftBottomHelpObj.GetComponent<Text>();
            bottomLeftHelpText.text = "(Left click to unit) Move";

            GameObject rightBottomHelpObj = Instantiate(textPrefab, bottomContainer);
            bottomRighttHelpText = rightBottomHelpObj.GetComponent<Text>();
            bottomRighttHelpText.text = "(Right click to unit) Actions";
        }

        void DestroyBottomHelp()
        {
            // Destroy bottom texts
            foreach (Transform child in bottomContainer) {
                Destroy(child.gameObject);
            }
        }

    }
}
