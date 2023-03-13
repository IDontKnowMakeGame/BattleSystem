using System;
using Block.Base;
using UnityEngine;

namespace Actor.Acts.BlockActs
{
    public class BlockRender : Act
    {
        [SerializeField] private Renderer _renderer;
        private BlockController _blockController;
        private Color _mainColor = Color.black;
        private Color _outlineColor = Color.black;

        protected override void Awake()
        {
            base.Awake();
            _blockController = _controller as BlockController;
        }

        public void SetMainColor(Color mainColor)
        {
            _mainColor = mainColor;
        }
        
        public void SetOutlineColor(Color outlineColor)
        {
            _outlineColor = outlineColor;
        }

        private void Update()
        {
            if (_blockController.IsActorOnBlock())
            {
                SetOutlineColor(Color.white);
            }
            else
            {
                SetOutlineColor(Color.black);
            }
        }

        private void LateUpdate()
        {
            Render();
        }

        public void Render()
        {
            _renderer.material.SetColor("_MainColor", _mainColor);
            _renderer.material.SetColor("_OutlineColor", _outlineColor);
        }
    }
}