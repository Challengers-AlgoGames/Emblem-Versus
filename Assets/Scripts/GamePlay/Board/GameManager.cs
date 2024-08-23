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
        public static Action OnGameStated;

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

            TurnBaseSystem.OnPhaseUpdate += OnTurnPhaseUpdated;
            InputHandler.OnEscapeKeyForCancelPressed += OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick += OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick += OnLeftClickModeListenGroundClick;
        }

        void OnDestroy()
        {
            TurnBaseSystem.OnPhaseUpdate -= OnTurnPhaseUpdated;
            InputHandler.OnEscapeKeyForCancelPressed -= OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick -= OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick -= OnLeftClickModeListenGroundClick;
        }

        void Start()
        {
            board.Generate();
            inputHandler.Active();
            moveSystem.Active();
            uIController.Active();
            turnBaseSystem.Active();
            OnGameStated?.Invoke();
        }

        void OnTurnPhaseUpdated(Commander _phase)
        {
            currentCameraController = cameraManager.SwitchCamera(_phase);
            inputHandler.Actualise();
            uIController.DisplayPhaseNotice(_phase);
        }

        void OnLeftClickModeListenUnitClick(Unit _unit, Vector3 _unitGroundCellPosition)
        {
            moveSystem.ClearActiveTiles();
            activeUnit = _unit;
            moveSystem.DisplayMoveRange(activeUnit.Mobility, _unitGroundCellPosition);

            currentCameraController.ZoomOut(_unit.transform.position);
            uIController.DisplayUnZoomTips();

            Dictionary<string, Button> actionsButtons = uIController.DisplayUnitActions();

            actionsButtons["attack"].onClick.AddListener(() => {
                Debug.Log("attack");
            });

            actionsButtons["wait"].onClick.AddListener(() => {
                Debug.Log("attack");
            });

            Debug.Log("done");
        }

        void OnLeftClickModeListenGroundClick(Vector3 _targetPosition)
        {
            if (activeUnit == null || activeUnit.Commander != turnBaseSystem.Phase)
            {
                return;
            }
            moveSystem.MoveUnit(activeUnit, _targetPosition);
            moveSystem.ClearActiveTiles();

            currentCameraController.UnZoom();
            uIController.DisplayMainTips();

            activeUnit = null;
        }

        void OnEscapeKeyForCancelPressed()
        {
            moveSystem.ClearActiveTiles();

            currentCameraController.UnZoom();
            uIController.DisplayMainTips();

            activeUnit = null;
        }
    }
}