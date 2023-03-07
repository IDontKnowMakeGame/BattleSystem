using System;
using Units.Behaviours.Default;

namespace Units.Behaviours.Character
{

    [Flags]
    public enum State
    {
        None = 0,
        Move = 1 << 0,
    }
    public class CharacterState : Behaviour
    {
        private State _state = State.None;


        public bool ContainState(State state)
        {
            return _state.HasFlag(state);
        }
        public void AddState(State state)
        {
            _state |= _state;
        }

        public void RemoveState(State state)
        {
            _state &= ~state;
        }
    }
}