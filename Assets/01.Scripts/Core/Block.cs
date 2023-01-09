using Unit;
using UnityEngine;

namespace Core
{
    public class Block : MonoBehaviour
    {
        #region Astar
        public GameObject tileOBJ;
        public int x;
        public int z;

        public bool isStart = false;
        public bool isEnd = false;
        public bool isWalkable = false;

        public int g;
        public int h;
        public Block parent;

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

        public bool ChangeStart
        {
            set
            {
                isStart = value;
            }
        }

        public bool ChangeEnd
        {
            set
            {
                isEnd = value;
            }
        }
        #endregion

        private Unit.Unit unitOnBlock;

        public Unit.Unit GetUnit()
        {
            return unitOnBlock;
        }
        public void MoveUnitOnBlock(Unit.Unit unit)
        {
            unitOnBlock = unit;
        }

        public void RemoveUnitOnBlock()
        {
            unitOnBlock = null;
        }

        private void Awake()
        {
            tileOBJ = this.gameObject;
            isWalkable = true;
            Vector3 pos = transform.position;
            x = (int)pos.x;
            z = (int)pos.z;  
        }
    }
}