using Acts.Base;
using Core;
using UnityEngine;

namespace Acts.Characters
{
    public class CharacterRender : Act
    {
        private Renderer _renderer;
        public override void Awake()
        {
            _renderer = ThisActor.GetComponentInChildren<Renderer>();
        }

        public override void LateUpdate()
        {
            BillBoard();
            base.LateUpdate();
        }

        public void BillBoard()
        {
            var cam = Define.MainCamera;
            var anchorTrm = ThisActor.transform.GetChild(0);
            
            var lookPos = anchorTrm.position - cam.transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            anchorTrm.rotation = rotation;
        }
    }
}