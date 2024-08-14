using UnityEngine;
using Units;
using System;

namespace UnitUI {
    public class UnitActionsUIController : MonoBehaviour
    {
        [SerializeField] private GameObject unitObj; // ui owner

        [Serializable] 
        public struct UnitActionUI {
            public GameObject button;
            public UnitAction action;
        };

        [SerializeField] private UnitActionUI[] unitActionsUI; // actions buttons showable

        private Unit unit; // unit script
        private bool uiIsActive; // ui state

        void Awake()
        {
            /* Deactive all ui */
            foreach (UnitActionUI item in unitActionsUI)
            {
                item.button.SetActive(false);
            }
        }

        void Start()
        {
            unit = unitObj.GetComponent<Unit>(); // get gived unit script for futur use
        }

        public void ShowUnitPossibleActionsUI()
        {
            /* Deactive UI if active */
            if(uiIsActive)
            {
                foreach (UnitActionUI item in unitActionsUI) {
                    item.button.SetActive(false);
                }

                uiIsActive = false;

                return;
            }

            /* Active UI if deactivate */
            foreach (UnitActionUI item in unitActionsUI) {
                if(item.action == UnitAction.ATTACK)
                {
                    if(unit.IsCanAttack) item.button.SetActive(true);
                } else {
                    item.button.SetActive(true);
                }

                uiIsActive = true;
            }
        }
    }
}