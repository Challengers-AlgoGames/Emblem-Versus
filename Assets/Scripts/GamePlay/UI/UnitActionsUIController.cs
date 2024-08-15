using UnityEngine;
using Units;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GamePlay.UIs {
    public class UnitActionsUIController : MonoBehaviour
    {
        [SerializeField] private GameObject owner; // ui owner
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonsContainer;

        [SerializeField] private UnitAction[] unitActionsUI; // actions buttons showable
        private Dictionary<UnitAction, string> unitActionsText = new Dictionary<UnitAction, string>() {
            {UnitAction.ATTACK, "Attack"},
            {UnitAction.SWITCH_WEAPON, "Switch Weapon"},
            {UnitAction.WAIT, "Wait"}
        };

        private Unit unit; // unit script
        private bool actionButttonIsActive; // ui state

        void Start()
        {
            unit = owner.GetComponent<Unit>(); // get gived unit script for futur use
        }

        public void ShowUnitPossibleActionsUI()
        {
            /* Destroy if instantiated */
            if(actionButttonIsActive)
            {
                foreach (Transform child in buttonsContainer)
                {
                    Destroy(child.gameObject);
                }
                actionButttonIsActive = false;
                return;
            }

            /* Active if not instantiated */
            for (int i = 0; i < unitActionsUI.Length; i++) {
                GameObject newButton = Instantiate(buttonPrefab, buttonsContainer);

                switch (unitActionsUI[i])
                {
                    case UnitAction.ATTACK:
                        if(unit.IsCanAttack)
                        {
                            newButton.GetComponentInChildren<Text>().text = unitActionsText[UnitAction.ATTACK];
                            newButton.GetComponent<Button>().onClick.AddListener(() => OnAttackButtonClicked());
                        } else {
                            Destroy(newButton);
                        }
                        break;
                    case UnitAction.SWITCH_WEAPON:
                        newButton.GetComponentInChildren<Text>().text = unitActionsText[UnitAction.SWITCH_WEAPON];
                        newButton.GetComponent<Button>().onClick.AddListener(() => OnSwitchWeaponButtonClicked());
                        break;
                    case UnitAction.WAIT:
                        newButton.GetComponentInChildren<Text>().text = unitActionsText[UnitAction.WAIT];
                        newButton.GetComponent<Button>().onClick.AddListener(() => OnWaitButtonClicked());
                        break;
                    default:
                        break;
                }
            }
            actionButttonIsActive = true;
        }

        void OnAttackButtonClicked() 
        {
            Debug.Log("Attack !");
        }

        void OnSwitchWeaponButtonClicked() 
        {
            Debug.Log("SwitchWeapon !");
        }

        void OnWaitButtonClicked() 
        {
            Debug.Log("Wait !");
        }
    }
}