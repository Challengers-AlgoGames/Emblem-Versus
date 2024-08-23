using System;
using Units;
using UnityEngine;

namespace GamePlay {
    public class GameManager : MonoBehaviour
    {
        public static Action OnGameStated;

        [SerializeField] MoveSystem moveSystem;
        [SerializeField] TurnBaseSystem turnBaseSystem;
        [SerializeField] Board board;

        private Unit activeUnit;

        void Awake()
        {
            
            InputHandler.OnReady += OnInputHandlerReady;
            InputHandler.OnEscapeKeyForCancelPressed += OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick += OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick += OnLeftClickModeListenGroundClick;
        }

        void OnDestroy()
        {
            InputHandler.OnReady -= OnInputHandlerReady;
            InputHandler.OnEscapeKeyForCancelPressed -= OnEscapeKeyForCancelPressed;
            InputHandler.OnLeftClickModeListenUnitClick -= OnLeftClickModeListenUnitClick;
            InputHandler.OnLeftClickModeListenGroundClick -= OnLeftClickModeListenGroundClick;
        }

        void OnInputHandlerReady()
        {
            board.Generate();
            OnGameStated?.Invoke();
        }

        void OnLeftClickModeListenUnitClick(Unit _unit, Vector3 _unitGroundCellPosition)
        {
            moveSystem.ClearActiveTiles();
            activeUnit = _unit;
            moveSystem.DisplayMoveRange(activeUnit.Mobility, _unitGroundCellPosition);
        }

        void OnLeftClickModeListenGroundClick(Vector3 _targetPosition)
        {
            if (activeUnit == null || activeUnit.Commander != turnBaseSystem.Phase)
            {
                moveSystem.ClearActiveTiles();
                return;
            }
            moveSystem.MoveUnit(activeUnit, _targetPosition);
            moveSystem.ClearActiveTiles();
            activeUnit = null;
        }

        void OnEscapeKeyForCancelPressed()
        {
            moveSystem.ClearActiveTiles();
            activeUnit = null;
        }
    }
}