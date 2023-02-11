using Core;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Wall
{
    [System.Serializable]    
    public class WallRender : UnitRender
    {
        public readonly int PosId = Shader.PropertyToID("_Position");
        public readonly int SizeId = Shader.PropertyToID("_Size");

        private Material thisMaterial;
        public LayerMask Mask;
        public int Size = 1;

        public override void Start()
        {
            thisMaterial = ThisBase.GetComponent<Renderer>().material;
        }

        protected override void Render()
        {
            var playerPos = InGame.PlayerBase.transform.position;
            var cam = Define.MainCam;
            var dir = cam.transform.position - playerPos;
            var ray = new Ray(playerPos, dir.normalized);

            if (Physics.Raycast(ray, 3000, Mask))
                thisMaterial.SetFloat(SizeId, Size);
            else
                thisMaterial.SetFloat(SizeId, 0);
            
            var view = cam.WorldToViewportPoint(playerPos);
            thisMaterial.SetVector(PosId, view);
        }
    }
}