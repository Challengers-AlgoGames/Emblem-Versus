using System.Collections;
using GamePlay.Cameras;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UIs
{
    /* Dependences : CameraController */
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private GameObject panelPrefab;
        [SerializeField] private GameObject buttonPrefab;

        [SerializeField] private Transform unitActionMenuContainer;
        [SerializeField] private Transform topLeftContainer;
        [SerializeField] private Transform bottomContainer;
        [SerializeField] private Transform phaseNoticeContainer;

        private Text topLeftHelpText;
        private Text bottomLeftHelpText;
        private Text bottomRighttHelpText;

        void Awake()
        {
            GameManager.OnGameStated += OnGameStated;
            CameraController.OnZoomOut += OnZoomOut;
            CameraController.OnUnZoom += OnUnZoom;
            TurnBaseSystem.OnPhaseUpdate += OnTurnPhaseUpdate;
        }

        void OnDestroy()
        {
            GameManager.OnGameStated -= OnGameStated;
            CameraController.OnZoomOut -= OnZoomOut;
            CameraController.OnUnZoom -= OnUnZoom;
            TurnBaseSystem.OnPhaseUpdate -= OnTurnPhaseUpdate;
        }

        public void OnGameStated()
        {
            GameObject leftHelpObj = Instantiate(textPrefab, topLeftContainer);
            topLeftHelpText = leftHelpObj.GetComponent<Text>();
            SetTopLeftText("(Esc) Menu");

            DisplayBottomHelp();
        }

        void OnTurnPhaseUpdate(Units.Commander _phase)
        {
            StartCoroutine(DisplayPhaseNotice(_phase, 2.5f));
        }

        IEnumerator DisplayPhaseNotice(Units.Commander _phase, float time)
        {
            GameObject phaseNoticePanel = Instantiate(panelPrefab, phaseNoticeContainer);

            GameObject phaseNoticeTextObj = Instantiate(textPrefab, phaseNoticePanel.transform);
            phaseNoticeTextObj.GetComponent<Text>().text = "Phase : " + _phase;

            yield return new WaitForSeconds(time);

            foreach (Transform child in phaseNoticeContainer) {
                Destroy(child.gameObject);
            }
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
