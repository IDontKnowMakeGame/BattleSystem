using Unit;
using UnityEngine;

namespace Core
{
    public class Block : MonoBehaviour
    {
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
    }
}