using Acts.Base;
using Core;
using UnityEngine;

namespace Acts.Characters
{
    [System.Serializable]
    public class CharacterRender : Act
    {
        [SerializeField] private Texture2D _defaultTexture;
        [SerializeField] private int _frame;
        private Renderer _renderer;
        public override void Awake()
        {
            _renderer = ThisActor.GetComponentInChildren<Renderer>();
            _renderer.material.SetTexture("_MainTex", _defaultTexture);
            var length = _defaultTexture.width / _frame;
            var tiling = new Vector2(1f / _frame, 1);
            _renderer.material.SetVector("_Tiling", tiling);
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