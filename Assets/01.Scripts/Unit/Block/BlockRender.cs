using System;
using UnityEngine;

namespace Unit.Block
{
    [Serializable]
    public class BlockRender : UnitRender
    {
        [SerializeField] private Color mainColor;
        [SerializeField] private Color outlineColor;
        [SerializeField] private float thickness = 0.01f;
        private Material thisMaterial;
        private static readonly int MainColor = Shader.PropertyToID("_MainColor");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");

        public override void Start()
        {
            thisMaterial = thisBase.GetComponent<Renderer>().material;
        }

        protected override void Render()
        {
            thisMaterial.SetColor(MainColor, mainColor);
            thisMaterial.SetColor(OutlineColor, outlineColor);
            thisMaterial.SetFloat(Thickness, thickness);
        }
        
        
    }
}