using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Base.Player
{
    public class PlayerInput : UnitInput
    {
        public UnitInputAction.PlayerActions playerActions { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            playerActions = InputActions.Player;
        }
    }
}
