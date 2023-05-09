using System;
using System.Collections;
using System.Linq;
using Actors;
using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Enemy;
using Actors.Characters.Player;
using Blocks.Acts;
using Core;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Blocks
{
    public class Block : Actor
    {
        #region Astar
        private GameObject tileOBJ;
        private int x;
        private int z;

        public bool isWalkable = false;
        public bool isMoving = false;

        public bool HasSwitchCamera
        {
            get {
                if (switchCamera == null)
                {
                    switchCamera = gameObject.GetComponent<SwitchCamera>();
                    if (switchCamera == null)
                    {
                        switchCamera = gameObject.AddComponent<SwitchCamera>();
                        switchCamera.enabled = false;
                    }
                }
                return switchCamera.enabled;
            }
        }

        public void SetWalkable(bool value)
        {
            isWalkable = value;
        }
        public bool canEnemyEnter = true;
        public bool canBossEnter = false;
        private int g;
        private int h;

        private Block parent = null;


        public GameObject TileOBJ { get => tileOBJ; }
        public int X { get => x; }
        public int Z { get => z; }
        public int G
        {
            get
            {
                return g;
            }
            set
            {
                g = value;
            }
        }
        public int H
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
            }
        }
        public Block Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public bool ChangeTile
        {
            set
            {
                isWalkable = value;
            }
        }

        public int fCost
        {
            get { return g + h; }
        }
        #endregion


        [SerializeField] private Actor _actorOnBlock;
        public Actor ActorOnBlock => _actorOnBlock;
        public bool IsActorOnBlock => ActorOnBlock != null;
        private BlockRender _blockRender;
        private BlockMovement _blockMovement;
        private BlockParticle _blockParticle;
        public Action<Vector3, float> OnHit = null;

		protected override void Init()
        {
            _blockRender = AddAct<BlockRender>();
            _blockMovement = AddAct<BlockMovement>();
            _blockParticle = AddAct<BlockParticle>();
            tileOBJ = this.gameObject;
            Vector3 pos = transform.position;
            x = (int)pos.x;
            z = (int)pos.z;
        }

        protected override void Awake()
        {
            base.Awake();
            UpdatePosition();
            InGame.AddBlock(this);
        }

        protected override void Update()
        {
            base.Update();
            if (IsActorOnBlock)
            {
                if (_actorOnBlock.Position != Position)
                    _actorOnBlock = null;
                if (_actorOnBlock == null)
                    return;
                if (ActorOnBlock.gameObject.activeSelf != true)
                    _actorOnBlock = null;
            }
        }

        public void SetActorOnBlock(Actor actor)
        {
            _actorOnBlock = actor;
        }

        public void ToggleIsWalkable()
        {
            isWalkable = !isWalkable;
        }
        
        public SwitchCamera switchCamera;
        public void ToggleHasSwitchCamera()
        {
            if (switchCamera.enabled)
            {
                switchCamera.enabled = false;
            }
            else
            {
                switchCamera.enabled = true;
            }
        }
        
        public void RemoveActorOnBlock()
        {
            _actorOnBlock = null;
        }

        public void Attack(float damage, Color color, float delay, Actor attacker, MovementType shakeType = MovementType.None, bool isLast = false, float strength = 0.5f)
        {
            StartCoroutine(AttackCoroutine(damage, color, delay, attacker, shakeType, isLast, strength));
        }

        private IEnumerator AttackCoroutine(float damage, Color color, float delay, Actor attacker, MovementType shakeType = MovementType.None, bool isLast = false, float strength = 0.5f)
        {
            //Debug.Log("/");
            var character = attacker as CharacterActor;
            var originalColor = Color.black;
            _blockRender.SetMainColor(color);
            yield return new WaitForSeconds(delay);
            _blockRender.SetMainColor(originalColor);
            Shake(delay / 2, shakeType, strength);
            var dir = (attacker.Position - Position).GetDirection();
            dir.y = 3f;
            var power = Vector3.Distance(attacker.Position, Position) * 10000;

            InGame.Actors.Values.Where(a => a is FlyableActor).Where(f => f.Position == Position).ToList().ForEach(o => ((FlyableActor)o).Fly(-dir, power));
            
            if(isLast)
                character.RemoveState(CharacterState.Attack);
            else
            {
                character.RemoveState(CharacterState.Hold);
            }
            
            if(_actorOnBlock == null)
                yield break;
            if(_actorOnBlock == attacker)
                yield break;

            var stat = _actorOnBlock.GetAct<CharacterStatAct>();
            stat?.Damage(damage, attacker);
        }
        
        public bool CheckActorOnBlock(Actor actor)
        {
            if (actor is PlayerActor)
                return true;
            if(actor is EnemyActor)
            {
                if(!canEnemyEnter)
                    return false;
            }
            if(actor is BossActor)
            {
                if(!canBossEnter)
                    return false;
            }

            return true;
        }

        public void Shake(float durantion, MovementType type, float strength = 1f, int vibrato = 10,
            float randomness = 90f)
        {
            switch (type)
            {
                case MovementType.Shake:
                {
                    _blockMovement.Shake(durantion, strength, vibrato, randomness);
                    _blockParticle.PlayExplosionParticle();
                    break;
                }
                case MovementType.Bounce:
                {
                    _blockMovement.Bounce(durantion, strength);
                    break;
                }
                case MovementType.Roll:
                {
                    _blockMovement.Roll(durantion, strength);
                    _blockParticle.PlaySmokeParticle();
                    break;
                }
                case MovementType.None:
                {
                    break;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (isWalkable)
            {
                
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position, Vector3.one);
            }
        }
    }
}