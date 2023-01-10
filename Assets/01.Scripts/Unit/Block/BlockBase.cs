using UnityEngine;

namespace Unit.Block
{
    public class BlockBase : UnitBase
    {
        #region Astar
        private GameObject tileOBJ;
        private int x;
        private int z;

        public bool isWalkable = false;
        private int g;
        private int h;

        private BlockBase parent;


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
        public BlockBase Parent 
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
        [SerializeField] private BlockRender blockRender;

        protected override void Init()
        {
            blockRender = AddBehaviour<BlockRender>(blockRender);
        }
        
        private Unit unitOnBlock;
        
        public Unit GetUnit()
        {
            return unitOnBlock;
        }
        public void MoveUnitOnBlock(Unit unit)
        {
            unitOnBlock = unit;
        }
        
        public void RemoveUnitOnBlock()
        {
            unitOnBlock = null;
        }
        protected override void Awake()
        {
            base.Awake();
            tileOBJ = this.gameObject;
            isWalkable = true;
            Vector3 pos = transform.position;
            x = (int)pos.x;
            z = (int)pos.z;

        }
    }
}