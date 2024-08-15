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
                ClearUIElements();
                UpdateUIState();
                return;
            }

            /* Active if not active */
            for (int i = 0; i < unitActionsUI.Length; i++) {
                GameObject newButton = Instantiate(buttonPrefab, buttonsContainer);
                
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
            UpdateUIState();
        }

        /* Main childs */
        void ShowSwitchWeaponMenu()
        {
            Inventory[] weapons = unit.Weapons;
            for (int i = 0; i < weapons.Length; i++)
            {
                GameObject newButton = Instantiate(buttonPrefab, buttonsContainer);
                newButton.GetComponentInChildren<Text>().text = weapons[i].isUsed ? weapons[i].item.gameObject.name+"(*)" : weapons[i].item.gameObject.name;
                newButton.GetComponent<Button>().onClick.AddListener(() => OnWeaponButtonClicked(newButton));
            }
        }

        /* Events Actions */
        void OnWeaponButtonClicked(GameObject button)
        {
            string weaponName = button.GetComponentInChildren<Text>().text.Replace("(*)", "");
            unit.SwitchWeapon(weaponName);

            ClearUIElements();
            UpdateUIState();
        }

        void OnAttackButtonClicked() 
        {
            ClearUIElements();
            // ShowWeaponsButton();
        }

        void OnSwitchWeaponButtonClicked() 
        {
            ClearUIElements();
            ShowSwitchWeaponMenu();
        }

        void OnWaitButtonClicked() 
        {
            unit.Wait();
            ClearUIElements();
            UpdateUIState();
        }

        /* Support Methods */
        void ClearUIElements()
        {

            foreach (Transform child in buttonsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        void UpdateUIState()
        {
            uiIsActive = !uiIsActive;
        }
    }
}