using System;
using DG.Tweening;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Unit.Block
{
    [Serializable]
    public class BlockRender : UnitRender
    {
        [SerializeField] private Color mainColor;
        [SerializeField] private Color outlineColor;
        [SerializeField] private float thickness = 0.1f;
        private Material thisMaterial;
        private static readonly int MainColor = Shader.PropertyToID("_MainColor");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");

        public override void Start()
        {
            thisMaterial = ThisBase.GetComponent<Renderer>().material;
        }

        protected override void Render()
        {
            thisMaterial.SetColor(MainColor, mainColor);
            thisMaterial.SetColor(OutlineColor, outlineColor);
            thisMaterial.SetFloat(Thickness, thickness);
            
            SetOutlineColor(Color.black);
        }
        
        public void SetMainColor(Color color)
        {
            mainColor = color;
        }
        
        public void DOSetMainColor(Color color, float duration)
        {
            thisMaterial.DOColor(color, MainColor, duration);
        }

        public void SetOutlineColor(Color color)
        {
            outlineColor = color;
        }
        
        public void DOSetOutlineColor(Color color, float duration)
        {
            thisMaterial.DOColor(color, OutlineColor, duration);
        }
    }
}