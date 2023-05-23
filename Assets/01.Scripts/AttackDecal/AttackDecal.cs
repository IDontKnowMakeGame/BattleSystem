using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Bases;
using Actors.Characters;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AttackDecals
{
    public class AttackDecal : MonoBehaviour
    {
        private Rect rect;
        private float damage = 0.0f;
        private CharacterActor attacker = null;
        private float fill = 1f;
        private DecalProjector decalProjector = null;
        private Material material = null;

        private void Awake()
        {
            decalProjector = GetComponent<DecalProjector>();
            material = decalProjector.material;
        }
        
        public void Attack(Rect _rect, CharacterActor _attacker, float _damage, float delay, bool isLast = false)
        {
            rect = _rect;
            decalProjector.size = new Vector3(rect.width, rect.height, 1.5f);
            attacker = _attacker;
            damage = _damage;
            fill = 1f;
            attacker.AddState(CharacterState.Hold);
            DOTween.To(() => fill, value => fill = value, 0, delay).OnUpdate(()=>material.SetFloat("_Fill", fill));
            StartCoroutine(AttackCoroutine(delay, isLast));
        }

        private IEnumerator AttackCoroutine(float delay, bool isLast)
        {
            yield return new WaitForSeconds(delay);

            var actors = from actor in InGame.Actors.Values
                where actor is CharacterActor
                let actorPos = new Vector3(actor.Position.x, actor.Position.z)
                where rect.Contains(actorPos)
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
    }
}