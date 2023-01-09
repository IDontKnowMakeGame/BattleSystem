using UnityEngine;

namespace Unit.Block
{
    public class BlockBase : UnitBase
    {
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
    }
}