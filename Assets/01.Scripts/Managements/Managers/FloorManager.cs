using _01.Scripts.Tools;
using Core;
using Managements.Managers.Base;
using Managements.Managers.Floor;
using Units.Base.Player;
using Units.Behaviours.Unit;

namespace Managements.Managers
{
    public class FloorManager : Manager
    {
        public IFloor CurrentFloor { get; set; }
        
        public override void Start()
        {
            //InGame.PlayerBase.SpawnPos = CurrentFloor.PlayerSpawnPos;
            //if(InGame.BossBase != null)
            //    InGame.BossBase.SpawnPos = CurrentFloor.BossSpawnPos;
        }

        public override void Update()
        {
            
        }
    }
}