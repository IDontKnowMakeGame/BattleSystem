using Acts.Base;
using UnityEngine;

namespace Blocks.Acts
{
    public class BlockRender : Act
    {
        private Renderer _renderer;
        private Block _block;
        
        public override void Awake()
        {
            _renderer = ThisActor.GetComponentInChildren<Renderer>();
            _block = ThisActor as Block;
        }

        public override void Update()
        {
            RenderIn();
        }

        public override void LateUpdate()
        {
            RenderOut();
        }

        
        public virtual void RenderIn()
        {
            SetOutlineColor(Color.black);   
        }
        
        public virtual void RenderOut()
        {
            if (_block.ActorOnBlock != null)
                SetOutlineColor(Color.white);
        }

        public Color GetMainColor()
        {
            return _renderer.material.GetColor("_MainColor");
        }
        public void SetMainColor(Color color)
        {
            _renderer.material.SetColor("_MainColor", color);
        }
        
        public Color GetOutlineColor()
        {
            return _renderer.material.GetColor("_OutlineColor");
        }

        public void SetOutlineColor(Color color)
        {
            _renderer.material.SetColor("_OutlineColor", color);
        }
        
    }
}