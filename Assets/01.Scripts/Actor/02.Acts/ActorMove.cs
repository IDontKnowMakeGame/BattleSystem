using UnityEngine;

namespace Actor.Acts
{
    public class ActorMove : Act
    {
        public void Translate(Vector3 dir)
        {
            transform.Translate(dir);
        }

        public void MoveTo(Vector3 pos)
        {
            
        }
    }
}