using System;
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
            string[] helpTexts = {
                "(Left click to unit) Move", 
                "(Right click to unit) Action"
            };
            SetBottomHelpTexts(helpTexts);
        }

        void OnTurnPhaseUpdate(Units.Commander _phase)
        {
            StartCoroutine(DisplayPhaseNotice(_phase, 2.5f));

            ClearContainer(topLeftContainer);

            GameObject leftHelpObj = Instantiate(textPrefab, topLeftContainer);
            topLeftHelpText = leftHelpObj.GetComponent<Text>();
            SetTopLeftText(_phase.ToString());
        }

        IEnumerator DisplayPhaseNotice(Units.Commander _phase, float time)
        {
            GameObject phaseNoticePanel = Instantiate(panelPrefab, phaseNoticeContainer);

            GameObject phaseNoticeTextObj = Instantiate(textPrefab, phaseNoticePanel.transform);
            phaseNoticeTextObj.GetComponent<Text>().text = "Phase : " + _phase;

            yield return new WaitForSeconds(time);

            ClearContainer(phaseNoticeContainer);
        }

        void OnZoomOut()
        {
            ClearContainer(bottomContainer);

            string[] helpTexts = {"(Esc) Cancel"};

            
            SetBottomHelpTexts(helpTexts);
        }

        void OnUnZoom()
        {
            ClearContainer(bottomContainer);

            string[] helpTexts = {
                "(Left click to unit) Move", 
                "(Right click to unit) Action"
            };
            SetBottomHelpTexts(helpTexts);
        }

        void SetTopLeftText(string text)
        {
            topLeftHelpText.text = text;
        }

        void SetBottomHelpTexts(string[] texts)
        {
            GameObject bottomHelpObj;

            foreach (string text in texts)
            {
                bottomHelpObj = Instantiate(textPrefab, bottomContainer);
                bottomLeftHelpText = bottomHelpObj.GetComponent<Text>();
                bottomLeftHelpText.text = text;
            }
        }

        void ClearContainer(Transform container)
        {
            foreach (Transform child in container) {
                Destroy(child.gameObject);
            }
        }

    }
}
