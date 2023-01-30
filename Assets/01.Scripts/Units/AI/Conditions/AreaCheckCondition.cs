using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class AreaCheckCondition : DetectCondition
    {
        private Vector3 _startPosition, _endPosition;
        
        protected override bool CheckConditionInternal()
        {
            if (_startPosition.x >= _target.Position.x && _target.Position.x >= _endPosition.x)
            {
                Debug.Log("First");
                if (_startPosition.z >= _target.Position.z && _target.Position.z >= _endPosition.z)
                {
                    Debug.Log("Second");
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