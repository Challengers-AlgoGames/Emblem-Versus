using UnityEngine;
using Units;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GamePlay.UIs {
    public class UnitActionsUIController : MonoBehaviour
    {
        [SerializeField] private GameObject owner; // ui owner
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform menuContainer;
        [SerializeField] private GameObject controllerButtonPrefab;
        [SerializeField] private Transform menuControllerContainer;
        [SerializeField] private UnitAction[] unitActionsUI; // actions menu showable
        private Dictionary<UnitAction, string> unitActionsText = new Dictionary<UnitAction, string>() {
            {UnitAction.ATTACK, "Attack"},
            {UnitAction.SWITCH_WEAPON, "Switch Weapon"},
            {UnitAction.WAIT, "Wait"}
        };

        private Unit unit; // unit component
        private bool uiIsActive; // ui state

        void Awake()
        {
            unit = owner.GetComponent<Unit>(); // get gived unit script for futur use
        }

        /* Main */
        public void ShowUnitActionsMenu()
        {
            if(unit.IsWasMoved) return; // can't do anathyng if unit was moved

            /* Destroy if active */
            if(uiIsActive)
            {
                return;
            }

            /* Active if not active */
            for (int i = 0; i < unitActionsUI.Length; i++) {
                GameObject newButton = Instantiate(buttonPrefab, menuContainer);
                
                // Show Menu on UI
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

            ShowCloseButton();
            UpdateUIState();
        }

        /* Main childs */
        void ShowSwitchWeaponMenu()
        {
            Inventory[] weapons = unit.Weapons;
            for (int i = 0; i < weapons.Length; i++)
            {
                GameObject newMenuElement = Instantiate(buttonPrefab, menuContainer);
                newMenuElement.GetComponentInChildren<Text>().text = weapons[i].isUsed ? weapons[i].item.gameObject.name+"(*)" : weapons[i].item.gameObject.name;
                newMenuElement.GetComponent<Button>().onClick.AddListener(() => OnWeaponButtonClicked(newMenuElement));
            }
            ShowBackButton();
        }

        void ShowBackButton()
        {
            foreach (Transform child in menuControllerContainer)
            {
                Destroy(child.gameObject);
            }
            
            GameObject backButton = Instantiate(controllerButtonPrefab, menuControllerContainer);
            backButton.GetComponentInChildren<Text>().text = "Back";
            backButton.GetComponent<Button>().onClick.AddListener(() => {
                ClearMenuUIElements();
                UpdateUIState(); // reset to false
                ClearMenuControllerUI();
                ShowUnitActionsMenu();
            });
        }

        void ShowCloseButton()
        {
            GameObject closeButton = Instantiate(controllerButtonPrefab, menuControllerContainer);
            closeButton.GetComponentInChildren<Text>().text = "Close";
            closeButton.GetComponent<Button>().onClick.AddListener(() => {
                ClearMenuUIElements();
                ClearMenuControllerUI();
                UpdateUIState();
            });
        }

        /* Events Actions */
        void OnWeaponButtonClicked(GameObject button)
        {
            string weaponName = button.GetComponentInChildren<Text>().text.Replace("(*)", "");
            unit.SwitchWeapon(weaponName);

            ClearMenuUIElements();
            ClearMenuControllerUI();
            UpdateUIState();
        }

        void OnAttackButtonClicked() 
        {
            ClearMenuUIElements();
            ClearMenuControllerUI();
            // ShowWeaponsButton();
        }

        void OnSwitchWeaponButtonClicked() 
        {
            ClearMenuUIElements();
            ClearMenuControllerUI();
            ShowSwitchWeaponMenu();
        }

        void OnWaitButtonClicked() 
        {
            unit.Wait();
            ClearMenuUIElements();
            ClearMenuControllerUI();
            UpdateUIState();
        }

        /* Support Methods */
        void ClearMenuUIElements()
        {

            foreach (Transform child in menuContainer) {
                Destroy(child.gameObject);
            }
        }

        void ClearMenuControllerUI()
        {
            foreach (Transform child in menuControllerContainer) {
                Destroy(child.gameObject);
            }
        }

        void UpdateUIState()
        {
            uiIsActive = !uiIsActive;
        }
    }
}