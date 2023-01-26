using Core;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    public class UnitRender : Behaviour
    {
        public override void Update()
        {
            Render();
        }

        protected virtual void Render()
        {
            Vector3 playerRotate = ThisBase.transform.rotation.eulerAngles;
            playerRotate.y = Define.MainCam.transform.rotation.eulerAngles.y;

            ThisBase.transform.rotation = Quaternion.Euler(playerRotate);
        }
    }
}