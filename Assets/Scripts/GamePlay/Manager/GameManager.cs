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
            FightSystem.OnAttackEnd += OnAttackEnd;
            TurnBaseSystem.OnPhaseUpdate += OnTurnPhaseUpdated;

            InputHandler.OnDisplayUnitActions += DisplayUnitWaitAction;
            InputHandler.OnLeftClickModeListenGroundClick += MoveUnit;
            InputHandler.OnLeftClickModeListenUnitClick += DisplayUnitMoveRange;
            InputHandler.OnEscapeKeyForCancelPressed += OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitActionClick += DisplayUnitAttackAction;
        }

        void OnDestroy()
        {
            Unit.OnWasMoved -= OnUnitWasMoved;
            FightSystem.OnAttackEnd -= OnAttackEnd;
            TurnBaseSystem.OnPhaseUpdate -= OnTurnPhaseUpdated;

            
            InputHandler.OnDisplayUnitActions -= DisplayUnitWaitAction;
            InputHandler.OnLeftClickModeListenGroundClick -= MoveUnit;
            InputHandler.OnLeftClickModeListenUnitClick -= DisplayUnitMoveRange;
            InputHandler.OnEscapeKeyForCancelPressed -= OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitActionClick -= DisplayUnitAttackAction;
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

        void OnAttackEnd(Unit _attackReceiver) //
        {
            if(_attackReceiver.Health == 0f)
            {
                Debug.Log("Unit death");
                board.ElimineUnit(_attackReceiver);
            }
            
            fightSystem.Clear();
            currentCameraController.UnZoom();
            uIController.DisplayMainTips();

            inputHandler.LeftClickListen(LeftClickInputMode.LISTEN_UNIT_CLICK);
            Debug.Log("attack end");
        }

        void OnEscapeKeyForCancelPressed()
        {
            moveSystem.ClearActiveTiles();
            fightSystem.Clear();
            currentCameraController.UnZoom();
            uIController.DisplayMainTips();
        }

        void DisplayUnitWaitAction(Unit _unit)
        {
            if(_unit.Commander != turnBaseSystem.Phase)
            {
                inputHandler.LeftClickListen(LeftClickInputMode.LISTEN_UNIT_CLICK);
                return;
            }

            activeUnit = _unit;
            HadleDisplayUnitWaitAction(activeUnit);
        }

        void DisplayUnitAttackAction(Unit _attackTarget)
        {
            Button attackButton = uIController.DisplayUnitAttackAction();

            attackButton.onClick.AddListener(() => {
                fightSystem.Attack(_attackTarget.transform.position);
                inputHandler.LeftClickListen(LeftClickInputMode.NONE);
            });
        }

        void OnUnitWasMoved()
        {
            HadleDisplayUnitWaitAction(activeUnit);
        }

        void HadleDisplayUnitWaitAction(Unit unit)
        {
            Button waitButton = uIController.DisplayUnitWaitAction();

            waitButton.onClick.AddListener(() => {
                currentCameraController.UnZoom();
                uIController.DisplayMainTips();

                activeUnit.Wait();

                inputHandler.LeftClickListen(LeftClickInputMode.LISTEN_UNIT_CLICK);
            });

            fightSystem.DetectEnemies(unit);
            fightSystem.DisplayTouchebleEnnemiesTile();

            currentCameraController.ZoomOut(unit.transform.position);
            uIController.DisplayUnZoomTips();

            inputHandler.LeftClickListen(LeftClickInputMode.LISTEN_UNIT_ACTION_CLICK);
        }
    }
}