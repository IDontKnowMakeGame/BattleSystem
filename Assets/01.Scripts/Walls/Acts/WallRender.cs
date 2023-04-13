using System;
using System.Reflection;
using Acts.Base;
using Core;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Walls.Acts
{
    [Serializable]
    public class WallRender : Act
    {

        public readonly int PosId = Shader.PropertyToID("_Position");
        public readonly int SizeId = Shader.PropertyToID("_Size");

        private Renderer renderer;
        private Material thisMaterial;
        public float Size = 1;

        public override void Awake()
        {
            renderer = ThisActor.GetComponentInChildren<Renderer>();
            thisMaterial = renderer.material;
            thisMaterial.SetFloat(SizeId, 0);
        }

        public override void Update()
        {
            thisMaterial.SetFloat(SizeId, 0);
        }

        public override void LateUpdate()
        {
            if(InGame.Player == null) return;
            var material = thisMaterial;
            var playerPos = InGame.Player.transform.position;
            var cam = Define.MainCamera;
            var view = cam.WorldToViewportPoint(playerPos);
            material.SetVector(PosId, view);
            renderer.material = material;
        }

        public void Invisible()
        {
            thisMaterial.SetFloat(SizeId, Size);
            Debug.Log(2);
        }
    }
}