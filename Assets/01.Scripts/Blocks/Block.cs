using Actors.Bases;
using Blocks.Acts;
using Core;
using UnityEngine;

namespace Blocks
{
    public class Block : Actor
    {
        #region Astar
        private GameObject tileOBJ;
        private int x;
        private int z;

        public bool isWalkable = false;
        public bool canEnemyEnter = true;
        public bool canBossEnter = false;
        private int g;
        private int h;

        private Block parent;


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

		protected override void Init()
        {
            AddAct<BlockRender>();
            tileOBJ = this.gameObject;
            isWalkable = true;
            Vector3 pos = transform.position;
            x = (int)pos.x;
            z = (int)pos.z;
        }

        protected override void Awake()
        {
            Position = transform.position;
            InGame.AddBlock(this);
            base.Awake();
        }

        public void SetActorOnBlock(Actor actor)
        {
            _actorOnBlock = actor;
        }
        
        public void RemoveActorOnBlock()
        {
            _actorOnBlock = null;
        }
    }
}