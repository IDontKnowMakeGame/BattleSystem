﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Player;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AttackDecals
{
    public class AttackDecal : MonoBehaviour
    {
        private Vector3 initPos;
        private Rect rect;
        private float damage = 0.0f;
        private CharacterActor attacker = null;
        private float fill = 1f;
        private bool isLast = false;
        private DecalProjector decalProjector = null;
        private Material material = null;
        private Sequence seq = null;

        private void Awake()
        {
            decalProjector = GetComponent<DecalProjector>();
            material = Instantiate(decalProjector.material);
        }
        
        public void Attack(Rect _rect, CharacterActor _attacker, float _damage, float delay, bool _isLast = true)
        {
            Init(_rect, _attacker, _damage, _isLast);
            attacker.AddState(CharacterState.Hold);
            seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => fill, value => fill = value, 0, delay).OnUpdate(()=>
            {
                if(material != null)
                    material.SetFloat("_Fill", fill);
            }));
            StartCoroutine(DeleteCoroutine());
            StartCoroutine(AttackCoroutine(delay));
        }

        public void AttackNoEnd(Rect _rect, CharacterActor _attacker, float _damage, bool _isLast = false)
        {
            Init(_rect, _attacker, _damage, _isLast);
            attacker.AddState(CharacterState.Hold);
            StartCoroutine(DeleteCoroutine());
        }

        private void Init(Rect _rect, CharacterActor _attacker, float _damage, bool _isLast = true)
        {
            rect = _rect;
            initPos = _attacker.Position;
            decalProjector.size = new Vector3(rect.width, rect.height, 1.5f);
            attacker = _attacker;
            damage = _damage;
            isLast = _isLast;
            fill = 1f;
            material.SetFloat("_Fill", fill);
            var decalColor = (_attacker is PlayerActor ? new Color(0, 0.7f, 0.5f) : Color.red) * 0.5f;
            material.SetColor("_MainColor", decalColor);
            decalProjector.material = material;
        }

        public void EndAttack()
        {
            var actors = from actor in InGame.Actors.Values
                where actor is CharacterActor
                let actorPos = new Vector3(actor.Position.x, actor.Position.z)
                where rect.Rotate(transform.eulerAngles.y).Contains(actorPos)
                where actor != attacker
                select actor as CharacterActor;
            
            foreach (var actor in actors)
            {
                if (actor == null)
                    continue;
                var stat = actor.GetAct<CharacterStatAct>();
                
                stat?.Damage(damage, attacker);
            }

            attacker.RemoveState(CharacterState.Hold);
            if (isLast)
            {
                attacker.RemoveState(CharacterState.Attack);
            }
            Define.GetManager<ResourceManager>().Destroy(this.gameObject);
        }

        private IEnumerator DeleteCoroutine()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (attacker.HasState(CharacterState.Die | CharacterState.KnockBack))
                {
                    attacker.RemoveState(CharacterState.Attack);
                    attacker.RemoveState(CharacterState.Hold);
                    Define.GetManager<ResourceManager>().Destroy(this.gameObject);
                    break;
                }
            }
        }

        private IEnumerator AttackCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            EndAttack();
        }
    }
}