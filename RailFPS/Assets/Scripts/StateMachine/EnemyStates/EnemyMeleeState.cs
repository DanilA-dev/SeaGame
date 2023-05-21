using Interfaces;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class EnemyMeleeState : BaseState
    {
        private Transform _myTransform;
        private Transform _playerTransform;
        private Transform _attackPoint;
        private float _attackRadius;
        private float _rotateSpeed;
        private float _damage;
        private float _moveSpeed;
        private float _additionalSpeed;
        private float _attackStopDistance;

        private LayerMask _playerLayer;
        private float _attackTime;
        
        public EnemyMeleeState(SimpleFSM simpleFsm, Transform myTransform, Transform playerTransform,
            float damage, float moveSpeed, float additionalSpeed,
            Transform attackPoint, float attackRadius,
            float attackStopDistance, float rotateSpeed, LayerMask playerLayer) : base(simpleFsm)
        {
            _myTransform = myTransform;
            _playerTransform = playerTransform;
            _damage = damage;
            _moveSpeed = moveSpeed;
            _attackPoint = attackPoint;
            _attackRadius = attackRadius;
            _additionalSpeed = additionalSpeed;
            _attackStopDistance = attackStopDistance;
            _rotateSpeed = rotateSpeed;
            _playerLayer = playerLayer;
        }

        public override void Enter()
        {
            base.Enter();
            _attackTime = 0.01f;
           
        }

        public override void Update()
        {
            MoveAndAttackPlayer();
            LookAtPlayer();
        }

        private void MoveAndAttackPlayer()
        {
            if (Vector3.Distance(_myTransform.position, _playerTransform.position) > _attackStopDistance)
            {
                _myTransform.position = Vector3.MoveTowards(_myTransform.position, _playerTransform.position,
                    Time.deltaTime * (_moveSpeed + _additionalSpeed));
            }
            else
            {
                if (_attackTime > 0)
                {
                    Collider[] colls = new Collider[1];
                    colls = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _playerLayer);
                    if (colls.Length > 0)
                    {
                        foreach (var c in colls)
                        {
                            if (c.TryGetComponent(out IDamagable damagable) && damagable.CanBeDamaged)
                                damagable.GetDamaged(_damage);
                        }
                    }

                    _attackTime -= Time.deltaTime;
                }
                else
                    FinishState();
            }
        }

        private void LookAtPlayer()
        {
            var dir = _playerTransform.position - _myTransform.position;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            _myTransform.rotation =
                Quaternion.RotateTowards(_myTransform.rotation, rot, _rotateSpeed * Time.deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
        }
        
    }
}