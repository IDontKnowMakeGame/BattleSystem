using Core;
using Managements.Managers;
using Unit.Block;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;
using Input = Managements.Managers.Input;

namespace Units.Base.Player
{
    public class PlayerBase : UnitBase
    {
        [SerializeField] private PlayerMove playerMove; 
        [SerializeField] private PlayerAttack PlayerAttack;
        [SerializeField] private PlayerEqiq PlayerEqiq;

        [SerializeField] private UnitAnimation unitAnimation;

        protected override void Init()
        {
            AddBehaviour(thisStat);
            AddBehaviour(unitAnimation);
            playerMove = AddBehaviour<PlayerMove>();
            PlayerAttack = AddBehaviour<PlayerAttack>();
            PlayerEqiq = AddBehaviour<PlayerEqiq>();
            base.Init();
        }

        protected override void Start()
        {
            Define.GetManager<InputManager>();
            base.Start();
        }

        protected override void Update()
        {
            var block = Define.GetManager<MapManager>().GetBlock(Position);
            block.GetBehaviour<BlockRender>().SetOutlineColor(Color.white);
            base.Update();
        }
    }
}