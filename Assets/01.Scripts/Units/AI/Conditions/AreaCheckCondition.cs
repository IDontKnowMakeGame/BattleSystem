using Core;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class AreaCheckCondition : DetectCondition
    {
        private Vector3 _startPosition, _endPosition;
        
        protected override bool CheckConditionInternal()
        {
            var max = new Vector3(Mathf.Max(_startPosition.x, _endPosition.x), 0, Mathf.Max(_startPosition.z, _endPosition.z)); 
            var min = new Vector3(Mathf.Min(_startPosition.x, _endPosition.x), 0, Mathf.Min(_startPosition.z, _endPosition.z)); 
            if (max.x >= _target.Position.x && _target.Position.x >= min.x)
            {
                if (max.z >= _target.Position.z && _target.Position.z >= min.z)
                {
                    Define.GetManager<EventManager>().TriggerEvent(EventFlag.ShowBossHP, new EventParam());
                    return true;
                }

            }
            return false;
        }
        
        public void SetArea(Vector3 startPosition, Vector3 endPosition)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
        }
    }
}