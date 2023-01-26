using Core;
using Units.Behaviours.Base;
using UnityEngine;

namespace Units.Behaviours.Unit
{
    public class UnitRender : UnitBehaviour
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