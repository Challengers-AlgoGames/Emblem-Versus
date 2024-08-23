using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Active()
        {
            DisplayMainTips();
        }

        public void DisplayPhaseNotice(Units.Commander _phase)
        {
            StartCoroutine(DisplayPhaseNotice(_phase, 2.5f));

            ClearContainer(topLeftContainer);
            ClearContainer(unitActionMenuContainer);

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

        public void DisplayUnZoomTips()
        {
            ClearContainer(bottomContainer);
            string[] helpTexts = {"(Esc) Cancel"};
            SetBottomHelpTexts(helpTexts);
        }

        public void DisplayMainTips()
        {
            ClearContainer(bottomContainer);
            
            ClearContainer(unitActionMenuContainer);

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

        public Dictionary<string, Button> DisplayUnitActions()
        {
            Dictionary<string, Button> buttons = new Dictionary<string, Button>();

            GameObject actionButton;
                
            actionButton = Instantiate(buttonPrefab, unitActionMenuContainer);
            actionButton.GetComponentInChildren<Text>().text = "Attack";
            buttons.Add("attack", actionButton.GetComponent<Button>());

            actionButton = Instantiate(buttonPrefab, unitActionMenuContainer);
            actionButton.GetComponentInChildren<Text>().text = "Wait";
            buttons.Add("wait", actionButton.GetComponent<Button>());

            return buttons;
        }

    }
}
