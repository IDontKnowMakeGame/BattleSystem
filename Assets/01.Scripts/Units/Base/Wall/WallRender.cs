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

        private Renderer renderer;
        private Material thisMaterial;
        public LayerMask Mask;
        public float Size = 1;

        public override void Start()
        {
            renderer = ThisBase.GetComponent<Renderer>();
            thisMaterial = renderer.material;
        }

        protected override void Render()
        {
            var material = thisMaterial;
            var playerPos = InGame.PlayerBase.transform.position;
            var cam = Define.MainCam;
            var dir = cam.transform.position - playerPos;
            
            material.SetFloat(SizeId, 0);
            for (var i = -2; i <= 2; i++)
            {
                dir = cam.transform.position - playerPos;
                var hits = Physics.RaycastAll(playerPos + new Vector3(i, 0), dir, 3000, Mask);
                foreach (var hit in hits)
                {
                    hit.collider.GetComponent<WallBase>().GetBehaviour<WallRender>().Invisible();
                }
            }

            var view = cam.WorldToViewportPoint(playerPos);
            material.SetVector(PosId, view);
            renderer.material = material;
        }
        
        public void Invisible()
        {
            thisMaterial.SetFloat(SizeId, Size);
        }
    }
}