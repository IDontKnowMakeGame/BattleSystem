using Actor.Bases;
using UnityEngine;

namespace Block.Base
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField] private Vector3 _position;
        public Vector3 Position
        {
            get => _position;
            set
            {
                value.y = 0;
                _position = value;
            }
        }

        [SerializeField] private ActorController _actorOnBlock;
        
        public void SetActorOnBlock(ActorController actor)
        {
            _actorOnBlock = actor;
        }
        
        
    }
}