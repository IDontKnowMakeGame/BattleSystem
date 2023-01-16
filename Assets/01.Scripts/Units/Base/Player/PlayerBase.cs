using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Player
{
    public class PlayerBase : UnitBase
    {
        [SerializeField] private PlayerMove playerMove; 
        [SerializeField] private PlayerAttack PlayerAttack; 
        public PlayerInput Input { get; private set; }
        protected override void Init()
        {
            Input = GetComponent<PlayerInput>();

            AddBehaviour(thisStat);
            playerMove = AddBehaviour<PlayerMove>();
            PlayerAttack = AddBehaviour<PlayerAttack>();

            AddInputActionsCallbacks();
        }

        #region Input
        private void AddInputActionsCallbacks()
        {
            Input.PlayerActions.Move.performed += playerMove.Translate;
        }

        private void RemoveInputActionsCallbacks()
        {
            Input.PlayerActions.Move.started -= playerMove.Translate;
        }

        public void OnApplicationQuit()
        {
            RemoveInputActionsCallbacks();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveInputActionsCallbacks();
        }
        #endregion
    }
}