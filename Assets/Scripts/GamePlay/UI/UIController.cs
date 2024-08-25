using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UIs
{
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

        public void Activate()
        {
            DisplayMainTips();
        }

        public void DisplayPhaseNotice(Commander _phase)
        {
            StartCoroutine(DisplayPhaseNotice(_phase, 2.5f));

            ClearContainer(topLeftContainer);
            ClearContainer(unitActionMenuContainer);

            GameObject leftHelpObj = Instantiate(textPrefab, topLeftContainer);
            topLeftHelpText = leftHelpObj.GetComponent<Text>();
            SetTopLeftText(_phase.ToString());
        }

        public void DisplayWinScreen(Commander _winner)
        {
            ClearContainer(bottomContainer);
            ClearContainer(phaseNoticeContainer);
            ClearContainer(phaseNoticeContainer);

            GameObject winNoticePanel = Instantiate(panelPrefab, phaseNoticeContainer);

            GameObject winNoticeTextObj = Instantiate(textPrefab, winNoticePanel.transform);
            winNoticeTextObj.GetComponent<Text>().text = "Winner : " + _winner;

            string[] helpTexts = {"PRESS (R) TO RESTART GAME"};
            SetBottomHelpTexts(helpTexts);
        }

        IEnumerator DisplayPhaseNotice(Commander _phase, float time)
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
                "(Z-Q-S-D) Move Camera",
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

        public Button DisplayUnitWaitAction()
        {
            ClearContainer(unitActionMenuContainer);

            GameObject waitButton = Instantiate(buttonPrefab, unitActionMenuContainer);
            waitButton.name = UnitAction.WAIT.ToString();
            waitButton.GetComponentInChildren<Text>().text = UnitAction.WAIT.ToString();

            return waitButton.GetComponent<Button>();
        }

        public Button DisplayUnitAttackAction()
        {
            ClearContainer(unitActionMenuContainer);
            
            GameObject attackButton = Instantiate(buttonPrefab, unitActionMenuContainer);
            attackButton.name = UnitAction.ATTACK.ToString();
            attackButton.GetComponentInChildren<Text>().text = UnitAction.ATTACK.ToString();
            
            return attackButton.GetComponent<Button>();
        }

    }
}
