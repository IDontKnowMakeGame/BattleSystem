using System;
using System.Collections;
using Acts.Base;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Acts.Characters
{
    [System.Serializable]
    public class CharacterRender : Act
    {
        [SerializeField] private bool isBillBoard = true;
        [Space]
        [SerializeField] private Texture2D _defaultTexture;
        [SerializeField] private int _frame;
        [SerializeField] private Vector3 size;
        [Space]
        [SerializeField] private int _blinkCnt = 1;
        [SerializeField] private float _blinkTime = 0.1f;
        [SerializeField] private float _blinkDelay = 0.1f;
        [SerializeField] private bool _isFloating = false;
        private readonly int _blinkOn = Shader.PropertyToID("_IsBlink");
        private Renderer _renderer;
        private Material currentMat;

        public Renderer Renderer => _renderer;
        public override void Awake()
        {
            _renderer = ThisActor.GetComponentInChildren<Renderer>();
            currentMat = _renderer.material;
            ThisActor.OnDirectionUpdate += SetDirection;
            // if (_defaultTexture != null)
            // {
            //     _renderer.material.SetTexture("_MainTex", _defaultTexture);
            //     var length = _defaultTexture.width / _frame;
            //     var tiling = new Vector2(1f / _frame, 1);
            //     _renderer.material.SetVector("_Tiling", tiling);
            // }
        }

        public override void Start()
        {
            base.Start();
            if (_isFloating)
            {
                ThisActor.transform.DOLocalMoveY(1.8f, 1.8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
        }

        public override void LateUpdate()
        {
            if(isBillBoard)
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

        public void Blink()
        {
            if (ThisActor == null) return;
            if (ThisActor.gameObject.activeInHierarchy == false) return;
            ThisActor.StartCoroutine(BlinkCoroutine());   
        }
        
        private IEnumerator BlinkCoroutine()
        {
            if(currentMat == null) yield break;
            for (var i = 0; i < _blinkCnt; i++)
            {
                currentMat.SetInt(_blinkOn, 1);
                yield return new WaitForSeconds(_blinkTime);
                currentMat.SetInt(_blinkOn, 0);
                yield return new WaitForSeconds(_blinkDelay);
            }
        }
        
        public void SetDirection(float direction)
        {
            ThisActor.SpriteTransform.localScale = new Vector3(size.x * -direction, size.y, size.z);
        }
    }
}