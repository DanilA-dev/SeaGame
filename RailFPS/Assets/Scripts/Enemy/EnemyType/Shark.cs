using StateMachine.EnemyStates;
using UnityEngine;

namespace Enemy
{
    public class Shark : BaseEnemy
    {
        [Header("MeleeState")] 
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _additionalSpeed;
        [SerializeField] private float _attackStopDistance;
        
        private EnemyMeleeState _meleeState;
        
        protected void Start()
        {
            _meleeState = new EnemyMeleeState(_simpleFsm, transform, _player,_attackDamage,_moveSpeed,
                _additionalSpeed, _attackPoint, _attackRadius,_attackStopDistance, _rotateSpeed,
                _playerLayer);
            
            _simpleFsm.AddState(_meleeState);
            _roamState.OnStateExit += () => _simpleFsm.ChangeState(_meleeState);
            _meleeState.OnStateExit += () => _simpleFsm.ChangeState(_startState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _roamState.OnStateExit -= () => _simpleFsm.ChangeState(_meleeState);
            _meleeState.OnStateExit -= () => _simpleFsm.ChangeState(_startState);
        }
    }

}
