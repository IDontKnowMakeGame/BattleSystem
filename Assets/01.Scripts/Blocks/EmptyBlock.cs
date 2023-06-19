using Actors.Characters;
using Core;
using UnityEngine;

namespace Blocks
{
    public class EmptyBlock : Block
    {
        protected bool isParent = false;
        private Transform anchorTrm;
        protected override void Start()
        {
            base.Start();
            anchorTrm = transform.Find("Anchor");
            CreateLight();
            CreateParticle();

        }

        protected override void Update()
        {
            base.Update();
            if (isParent)
                return;
            if(ActorOnBlock == null) return;
            if (ActorOnBlock.Position == Position)
            {
                if (ActorOnBlock is CharacterActor)
                {
                    var characterOnBlock = ActorOnBlock as CharacterActor;
                    if (characterOnBlock.HasState(CharacterState.Die))
                        return;
                }
                var stat = ActorOnBlock.GetAct<CharacterStatAct>();
                stat?.Damage(int.MaxValue, this);
            }
        }
        
        private void CreateLight()
        {
            var lightObj = new GameObject("Light");
            var light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.range = 1.5f;
            light.intensity = 5f;
            light.color = Color.white;
            light.shadows = LightShadows.Hard;
            light.renderMode = LightRenderMode.ForceVertex;
            lightObj.transform.SetParent(anchorTrm);
            lightObj.transform.localPosition = Vector3.up * -0.2f;
        }

        private void CreateParticle()
        {
            var resource = Define.GetManager<ResourceManager>();
            var particle = resource.Instantiate("EB_Particle");
            particle.transform.SetParent(anchorTrm);
            particle.transform.localPosition = Vector3.up * 0.25f;
        }
    }
}