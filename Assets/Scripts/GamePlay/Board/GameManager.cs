using System;
using System.Collections.Generic;
using GamePlay.Cameras;
using GamePlay.UIs;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay {
    public enum CameraUnZoomJustification
    {
        NONE,
        UNIT_WAIT,
        UNIT_ATTACK,
        UNIT_MOVE
    }

    public class GameManager : MonoBehaviour
    {
        public static Action OnUnitAttackButtonWasClicked;
        public static Action OnUnitWaitButtonWasClicked;
        public static Action OnDisplayUnitActionAborded;

        [SerializeField] MoveSystem moveSystem;
        [SerializeField] InputHandler inputHandler;
        [SerializeField] TurnBaseSystem turnBaseSystem;
        [SerializeField] UIController uIController;
        [SerializeField] CameraManager cameraManager;
        [SerializeField] Board board;

        private Unit activeUnit;
        private CameraController currentCameraController;
        private CameraUnZoomJustification cameraUnZoomJustification = CameraUnZoomJustification.NONE;
        

        void Awake()
        {
            Unit.OnWasMoved += OnUnitWasMoved;
            TurnBaseSystem.OnPhaseUpdate += OnTurnPhaseUpdated;
            CameraController.OnUnZoomWasPerformed += OnCameraUnZoomWasPerformed;

            InputHandler.OnDisplayUnitActions += OnDisplayUnitActions;
            InputHandler.OnEscapeKeyForCancelPressed += OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick += OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick += OnLeftClickModeListenGroundClick;
        }

        void OnDestroy()
        {
            Unit.OnWasMoved -= OnUnitWasMoved;
            TurnBaseSystem.OnPhaseUpdate -= OnTurnPhaseUpdated;
            CameraController.OnUnZoomWasPerformed -= OnCameraUnZoomWasPerformed;

            InputHandler.OnDisplayUnitActions -= OnDisplayUnitActions;
            InputHandler.OnEscapeKeyForCancelPressed -= OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick -= OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick -= OnLeftClickModeListenGroundClick;
        }

        void Start()
        {
            board.Generate();
            moveSystem.Active();
            uIController.Active();
            turnBaseSystem.Active();
        }

        void OnCameraUnZoomWasPerformed()
        {
            uIController.DisplayMainTips();

            switch(cameraUnZoomJustification)
            {
                case CameraUnZoomJustification.UNIT_WAIT:
                    activeUnit.Wait();
                    break;
                case CameraUnZoomJustification.UNIT_ATTACK:
                    break;
                case CameraUnZoomJustification.UNIT_MOVE:
                    break;
                case CameraUnZoomJustification.NONE:
                    break;
                default:
                    Debug.Log("Not supported");
                    break;
            }

            activeUnit = null;
            cameraUnZoomJustification = CameraUnZoomJustification.NONE;
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
            cameraUnZoomJustification = CameraUnZoomJustification.NONE;
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
            cameraUnZoomJustification = CameraUnZoomJustification.UNIT_MOVE;
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
                cameraUnZoomJustification = CameraUnZoomJustification.UNIT_WAIT;

                OnUnitWaitButtonWasClicked?.Invoke();
            });

            currentCameraController.ZoomOut(unit.transform.position);
            uIController.DisplayUnZoomTips();
        }
    }
}