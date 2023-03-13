using System;
using Actor.Bases;
using ControllerBase;
using Core;
using Managements.Managers;
using UnityEngine;

namespace Block.Base
{
    public class BlockController : Controller
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

        private BlockController parent;


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
        public BlockController Parent
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


        [SerializeField] private ActorController _actorOnBlock;

        private void Awake()
        {
            Position = transform.position;
            Define.GetManager<MapManager>().BlockDictionary.Add(Position, this);
        }

        public void SetActorOnBlock(ActorController actor)
        {
            _actorOnBlock = actor;
        }
        
        public ActorController GetActorOnBlock()
        {
            return _actorOnBlock;
        }
        
        public bool IsActorOnBlock()
        {
            return _actorOnBlock != null;
        }
    }
}