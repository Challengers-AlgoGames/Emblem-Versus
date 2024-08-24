using System;
using System.Collections.Generic;
using GamePlay.Cameras;
using GamePlay.UIs;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay {
    public class GameManager : MonoBehaviour
    {
        public static Action OnUnitAttackButtonWasClicked;
        public static Action OnUnitWaitButtonWasClicked;
        public static Action OnDisplayUnitActionAborded;

        [SerializeField] TileSystem tileSystem;
        [SerializeField] MoveSystem moveSystem;
        [SerializeField] InputHandler inputHandler;
        [SerializeField] TurnBaseSystem turnBaseSystem;
        [SerializeField] UIController uIController;
        [SerializeField] CameraManager cameraManager;
        [SerializeField] Board board;

        private Unit activeUnit;
        private CameraController currentCameraController;

        void Awake()
        {
            Unit.OnWasMoved += OnUnitWasMoved;
            TurnBaseSystem.OnPhaseUpdate += OnTurnPhaseUpdated;

            InputHandler.OnDisplayUnitActions += OnDisplayUnitActions;
            InputHandler.OnEscapeKeyForCancelPressed += OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick += OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick += OnLeftClickModeListenGroundClick;
        }

        void OnDestroy()
        {
            Unit.OnWasMoved -= OnUnitWasMoved;
            TurnBaseSystem.OnPhaseUpdate -= OnTurnPhaseUpdated;

            InputHandler.OnDisplayUnitActions -= OnDisplayUnitActions;
            InputHandler.OnEscapeKeyForCancelPressed -= OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick -= OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick -= OnLeftClickModeListenGroundClick;
        }

        void Start()
        {
            board.Generate();
            board.PlaceUnits();
            Debug.Log("unit placed");
            tileSystem.Active(board.GetTilemap());
            moveSystem.Active(board.GetGrid());
            uIController.Active();
            turnBaseSystem.Active(board);
        }

        void OnTurnPhaseUpdated(Commander _phase)
        {
            currentCameraController = cameraManager.SwitchCamera(_phase);
            inputHandler.Actualise();
            uIController.DisplayPhaseNotice(_phase);
        }

        void OnLeftClickModeListenUnitClick(Unit _unit, Vector3 _unitGroundCellPosition)
        {
            activeUnit = _unit;

            moveSystem.ClearActiveTiles();
            moveSystem.DisplayMoveRange(activeUnit.Mobility, _unitGroundCellPosition);

            currentCameraController.ZoomOut(_unit.transform.position);
            uIController.DisplayUnZoomTips();
        }

        void OnLeftClickModeListenGroundClick(Vector3 _targetPosition)
        {
            if (activeUnit == null || activeUnit.Commander != turnBaseSystem.Phase)
                return;

            moveSystem.MoveUnit(activeUnit, _targetPosition);
            moveSystem.ClearActiveTiles();
        }

        void OnEscapeKeyForCancelPressed()
        {
            moveSystem.ClearActiveTiles();
            currentCameraController.UnZoom();
            uIController.DisplayMainTips();
        }

        void OnDisplayUnitActions(Unit _unit)
        {
            if(_unit.Commander != turnBaseSystem.Phase)
            {
                OnDisplayUnitActionAborded?.Invoke();
                return;
            }
                

            activeUnit = _unit;
            HadleDisplayUnitActionMenu(activeUnit);
        }

        void OnUnitWasMoved ()
        {
            HadleDisplayUnitActionMenu(activeUnit);
        }

        void HadleDisplayUnitActionMenu(Unit unit)
        {
            Dictionary<UnitAction, Button> actionsButtons = uIController.DisplayUnitActions();

            actionsButtons[UnitAction.ATTACK].onClick.AddListener(() => {
                Debug.Log("attack");
                OnUnitAttackButtonWasClicked?.Invoke();
            });

            actionsButtons[UnitAction.WAIT].onClick.AddListener(() => {
                currentCameraController.UnZoom();
                uIController.DisplayMainTips();
                activeUnit.Wait();
                OnUnitWaitButtonWasClicked?.Invoke();
            });

            currentCameraController.ZoomOut(unit.transform.position);
            uIController.DisplayUnZoomTips();
        }
    }
}