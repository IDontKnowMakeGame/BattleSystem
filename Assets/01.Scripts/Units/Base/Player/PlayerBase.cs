using Core;
using Managements.Managers;
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
        protected override void Init()
        {
            AddBehaviour(thisStat);
            playerMove = AddBehaviour<PlayerMove>();
            PlayerAttack = AddBehaviour<PlayerAttack>();
        }

        protected override void Start()
        {
            Define.GetManager<InputManager>().InitInGameInput(InputTarget.Left, new Input(){
                action = () =>
                {
                    Define.GetManager<InputManager>().ChangeInGameKey(InputTarget.Left, KeyCode.O);
                    Debug.Log("Left");
                },
                keyCode = KeyCode.A,
                inputStatus = InputStatus.Press}
            );
            base.Start();
        }
    }
}