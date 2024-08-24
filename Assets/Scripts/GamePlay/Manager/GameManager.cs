using System;
using System.Collections.Generic;
using GamePlay.Cameras;
using GamePlay.Sys;
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
        
        [SerializeField] Board board;
        [SerializeField] InputHandler inputHandler;
        [SerializeField] UIController uIController;
        [SerializeField] CameraManager cameraManager;
        [SerializeField] MoveSystem moveSystem;
        [SerializeField] TileSystem tileSystem;
        [SerializeField] FightSystem fightSystem;
        [SerializeField] TurnBaseSystem turnBaseSystem;

        private Unit activeUnit;
        private CameraController currentCameraController;

        void Awake()
        {
            Unit.OnWasMoved += OnUnitWasMoved;
            TurnBaseSystem.OnPhaseUpdate += OnTurnPhaseUpdated;

            InputHandler.OnDisplayUnitActions += DisplayUnitActions;
            InputHandler.OnEscapeKeyForCancelPressed += OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick += DisplayUnitMoveRange;
            InputHandler.OnLeftClickModeListenGroundClick += MoveUnit;
        }

        void OnDestroy()
        {
            Unit.OnWasMoved -= OnUnitWasMoved;
            TurnBaseSystem.OnPhaseUpdate -= OnTurnPhaseUpdated;

            InputHandler.OnDisplayUnitActions -= DisplayUnitActions;
            InputHandler.OnEscapeKeyForCancelPressed -= OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick -= DisplayUnitMoveRange;
            InputHandler.OnLeftClickModeListenGroundClick -= MoveUnit;
        }

        void Start()
        {
            board.Generate();
            board.PlaceUnits();
            tileSystem.Activate(board.GetTilemap());
            moveSystem.Activate(board.GetGrid());
            fightSystem.Activate(tileSystem,board.GetGrid());
            uIController.Activate();
            turnBaseSystem.Activate(board);
        }

        void OnTurnPhaseUpdated(Commander _phase)
        {
            currentCameraController = cameraManager.SwitchCamera(_phase);
            inputHandler.Actualise();
            uIController.DisplayPhaseNotice(_phase);
        }

        void DisplayUnitMoveRange(Unit _unit, Vector3 _unitGroundCellPosition)
        {
            activeUnit = _unit;

            moveSystem.ClearActiveTiles();
            moveSystem.DisplayMoveRange(activeUnit.Mobility, _unitGroundCellPosition);

            currentCameraController.ZoomOut(_unit.transform.position);
            uIController.DisplayUnZoomTips();
        }

        void MoveUnit(Vector3 _targetPosition)
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

        void DisplayUnitActions(Unit _unit)
        {
            if(_unit.Commander != turnBaseSystem.Phase)
            {
                OnDisplayUnitActionAborded?.Invoke();
                return;
            }
                

            activeUnit = _unit;
            HadleDisplayUnitActionMenu(activeUnit);
            fightSystem.DetectEnemies(activeUnit);
        }

        void OnUnitWasMoved()
        {
            HadleDisplayUnitActionMenu(activeUnit);
            fightSystem.DetectEnemies(activeUnit);
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