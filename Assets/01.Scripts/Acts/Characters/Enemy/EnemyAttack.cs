using System.Collections.Generic;
using System.Numerics;
using Actors.Characters;
using Actors.Characters.Enemy;
using Acts.Base;
using AttackDecals;
using Blocks.Acts;
using Core;
using Data;
using Managements.Managers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using Vector3 = UnityEngine.Vector3;

namespace Acts.Characters.Enemy
{
    public class EnemyAttack : Act
    {
        protected CharacterActor CharacterActor => ThisActor as CharacterActor;
        protected ItemInfo DefaultStat => ThisActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;

        protected void Attack()
        {
            CharacterActor.AddState(CharacterState.Attack);
        }

        public void DefaultAttack(Vector3 dir, bool isLast = true)
        {
            Attack();
            //Define.GetManager<MapManager>().AttackBlock(ThisActor.Position + dir, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, isLast);
            if(ThisActor is BossActor)
            {
                //Define.GetManager<SoundManager>().PlayAtPoint("Boss/explosion", ThisActor.Position + dir, 1);
            }
            InGame.Attack(ThisActor.Position + dir, new Vector3(1, 0, 1), DefaultStat.Atk, DefaultStat.Ats, CharacterActor, isLast);
        }
        
        public void HorizontalAttack(Vector3 dir, bool isLast = true)
        {
            Attack();
            var degree = dir.ToDegree().GetRotation();
            Debug.Log(degree);
            var range = new Vector3[] { new (1, 0, -1), new (1, 0, 0), new (1, 0, 1) };
            for (var r = 0; r < 3; r++)
            {
                var attackPos = CharacterActor.Position + (degree * range[r]);
                //Define.GetManager<MapManager>()
                //    .AttackBlock(attackPos, DefaultStat.Atk, 0.1f, CharacterActor, MovementType.None);
                InGame.Attack(attackPos, new Vector3(1, 0, 1), DefaultStat.Atk, 0.1f, CharacterActor);

                //Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats,
                //    CharacterActor, MovementType.None, isLast);
            }
            InGame.Attack(CharacterActor.Position, new Vector3(1, 0, 1), DefaultStat.Atk, DefaultStat.Ats, CharacterActor, isLast);
        }

        public List<AttackDecal> HorizontalAttackNoEnd(Vector3 dir, bool isLast = true)
        {
            Attack();
            var degree = dir.ToDegree().GetRotation();
            var decals = new List<AttackDecal>(); 
            Debug.Log(degree);
            var range = new Vector3[] { new (1, 0, -1), new (1, 0, 0), new (1, 0, 1) };
            for (var r = 0; r < 3; r++)
            {
                var attackPos = CharacterActor.Position + (degree * range[r]);
                //Define.GetManager<MapManager>()
                //    .AttackBlock(attackPos, DefaultStat.Atk, 0.1f, CharacterActor, MovementType.None);
                decals.Add(InGame.AttackNoEnd(attackPos, new Vector3(1, 0, 1), DefaultStat.Atk, CharacterActor));

                //Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats,
                //    CharacterActor, MovementType.None, isLast);
            }
            decals.Add(InGame.AttackNoEnd(CharacterActor.Position, new Vector3(1, 0, 1), DefaultStat.Atk, CharacterActor, isLast));

            return decals;
        }

        public void VerticalAttack(Vector3 dir, bool isLast = true)
        {
            Attack();
            var degree = dir.ToDegree().GetRotation();
            var range = Vector3.right;
            for (var r = 1; r <= 5; r++)
            {
                var attackPos = CharacterActor.Position + (degree * range * r);
                //Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor);
                InGame.Attack(attackPos, new Vector3(1, 0, 1), DefaultStat.Atk, DefaultStat.Ats, CharacterActor);
            }
            
            //Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, isLast);
            InGame.Attack(CharacterActor.Position, new Vector3(1, 0, 1), DefaultStat.Atk, DefaultStat.Ats, CharacterActor, isLast);
        }

        public void ForwardAttack(Vector3 dir, bool isLast = true)
        {
            Attack();
            var degree = dir.ToDegree().GetRotation();
            for(var i = -1; i <= 1; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if(i == 0 && j == 0) continue;
                    var attackPos = CharacterActor.Position + (degree * new Vector3(j, 0, i));
                    //Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor);   
                    InGame.Attack(attackPos, new Vector3(1, 0, 1), DefaultStat.Atk, DefaultStat.Ats, CharacterActor);
                }
            }
            //Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, isLast)
            InGame.Attack(CharacterActor.Position, new Vector3(1, 0, 1), DefaultStat.Atk, DefaultStat.Ats, CharacterActor, isLast);
        }
        
        public void SliceEffect(Vector3 dir)
        {
            var particle = new MeshParticle.Particle
            {
                randomProperties = new MeshParticle.ParticleRandomProperties()
                {
                    minPosition = new Vector3(-0.3f, -0.3f),
                    maxPosition = new Vector3(0.3f, 0.3f),
                    minRotation = 0,
                    maxRotation = 0,
                    minQuadSize = new Vector3(1.5f, 1.5f),
                    maxQuadSize = new Vector3(2f, 2f),
                }
            };
            var pos = ThisActor.transform.position + dir;
            var rot = InGame.CamDirCheck(-dir).ToDegree();
            particle.position = new Vector3(pos.x, pos.z) + particle.randomProperties.RandomPos;
            particle.rotation = rot + 30;
            particle.quadSize = particle.randomProperties.RandomQuadSize;
            particle.skewed = true;
            particle.uvIndex = Random.Range(1, 2);
            MeshParticle.Instance.AddParticle("Slice", particle);
        }
    }
}