using Core;
using Managements.Managers;
using Unit.Block;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Units.Base.Player
{
    public class PlayerBase : UnitBase
    {
        [SerializeField] private PlayerMove playerMove; 
        [SerializeField] private PlayerAttack PlayerAttack;
        [SerializeField] private PlayerEqiq PlayerEqiq;
        [SerializeField] private UnitAnimation unitAnimation;
        [SerializeField] private PlayerBuff playerBuff;
        [SerializeField] private PlayerStat thisStat;      
        [SerializeField] private CharacterRender characterRender;

        protected override void Init()
        {
            InGame.PlayerBase = this;
            AddBehaviour(thisStat);
            AddBehaviour(unitAnimation);
            AddBehaviour(playerMove);
            AddBehaviour(PlayerAttack);
            AddBehaviour(playerBuff);
            AddBehaviour(PlayerEqiq);
            base.Init();
            ChangeBehaviour(characterRender);
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
            
            if(Input.GetKeyDown(KeyCode.Space))
                characterRender.DamageRender();
        }
    }
}