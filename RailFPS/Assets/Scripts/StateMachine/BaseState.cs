using System;

namespace StateMachine
{
    public abstract class BaseState
    {
        public event Action OnStateEnter;
        public event Action OnStateExit;

        
        protected readonly SimpleFSM SimpleFsm;
        
        public BaseState(SimpleFSM simpleFsm)
        {
            SimpleFsm = simpleFsm;
        }

        public virtual void Enter()
        {
            OnStateEnter?.Invoke();
        }
        public virtual void Update() {}

        public virtual void Exit() {}

        public void FinishState()
        {
            OnStateExit?.Invoke();
        }
        
        public virtual void DrawGizmo() {}
    }
}