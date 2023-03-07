using Core;
using UnityEngine;
using Behaviour = Units.Behaviours.Default.Behaviour;

namespace Units.Behaviours.Unit
{
    public class UnitRender : Behaviour
    {
        protected Transform anchorTrm = null;
        protected Renderer modelRdr = null;
        
        public override void Awake()
        {
            anchorTrm = ThisBase.transform.Find("Anchor");
            modelRdr = anchorTrm.Find("Model").GetComponent<Renderer>();
        }

        public override void LateUpdate()
        {
            Render();
        }

        protected virtual void Render()
        {
              Billboard();
        }

        protected virtual void Billboard()
        {
            var camPos = Define.MainCamera.transform.position;
            var lookAt = anchorTrm.position - camPos;
            anchorTrm.LookAt(lookAt);
            anchorTrm.rotation = Quaternion.Euler(30, anchorTrm.rotation.eulerAngles.y, anchorTrm.rotation.eulerAngles.z);
        }
    }
}