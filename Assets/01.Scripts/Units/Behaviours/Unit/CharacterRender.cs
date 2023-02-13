using System.Collections;
using UnityEngine;

namespace Units.Behaviours.Unit
{
    [System.Serializable]
    public class CharacterRender : UnitRender
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private int _blinkCnt = 0;
        [SerializeField] private float _blinkTime = 0f;
        [SerializeField] private float _blinkDelay = 0f;
        private Material damageMat;
        private readonly int _whiteOn = Shader.PropertyToID("_On");

        public override void Awake()
        {
            damageMat = _renderer.materials[1];
        }

        public void DamageRender()
        {
            ThisBase.StartCoroutine(DamageRenderCoroutine());   
        }
        
        private IEnumerator DamageRenderCoroutine()
        {
            for (var i = 0; i < _blinkCnt; i++)
            {
                damageMat.SetFloat(_whiteOn, 1);
                yield return new WaitForSeconds(_blinkTime);
                damageMat.SetFloat(_whiteOn, 0);
                yield return new WaitForSeconds(_blinkDelay);
            }
        }
    }
}