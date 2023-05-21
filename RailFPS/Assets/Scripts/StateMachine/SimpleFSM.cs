
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public class SimpleFSM
    {
        public BaseState CurrentState { get; private set; }
        
        private Dictionary<Type, BaseState> _states = new Dictionary<Type, BaseState>();

        public void AddState(BaseState newState)
        {
            _states.Add(newState.GetType(), newState);
        }

        public void ChangeState(BaseState state)
        {
            if (_states.ContainsValue(state))
            {
                CurrentState?.Exit();
                CurrentState = state;
                CurrentState?.Enter();  
            }
        }

        public void UpdateState()
        {
            CurrentState?.Update();
        }
    }
}