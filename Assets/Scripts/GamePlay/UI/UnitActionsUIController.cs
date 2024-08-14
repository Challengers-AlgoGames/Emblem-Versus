using UnityEngine;
using Units;
using System;

namespace UnitUI {
    public enum UnitAction
    {
        ATTACK,
        SWITCH_WEAPON,
        WAIT
    }
    public class UnitActionsUIController : MonoBehaviour
    {
        [SerializeField] private GameObject unitObj;

        [Serializable] 
        public struct UnitActionUI {
            public GameObject button;
            public UnitAction action;
        };

        [SerializeField] private UnitActionUI[] unitActionsUI;

        private Unit unit;
        private bool uiIsActive;

        void Awake()
        {
            foreach (UnitActionUI item in unitActionsUI)
            {
                item.button.SetActive(false);
            }
        }

        void Start()
        {
            unit = unitObj.GetComponent<Unit>();
        }

        public void ShowUnitPossibleActionsUI()
        {
            if(uiIsActive)
            {
                foreach (UnitActionUI item in unitActionsUI) {
                    item.button.SetActive(false);
                }

                uiIsActive = false;

                return;
            }

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