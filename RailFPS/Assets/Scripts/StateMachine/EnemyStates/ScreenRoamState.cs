using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class ScreenRoamState : BaseState
    {
        private float _stoppingDistance;
        private float _roamTime;
        private float _moveSpeed;
        private Transform _myTransform;
        private Transform _playerTransform;
        private float _rotateSpeed;
        private readonly List<EnemyMovePoint> _roamPoints;
        
        private float _currentRoamTime;
        private int _pointIndex;
        
        public ScreenRoamState(SimpleFSM simpleFsm, Transform myTransform, Transform playerTransform,
            float rotateSpeed, float moveSpeed, float roamTime,
            float stoppingDistance, List<EnemyMovePoint> roamPoints) : base(simpleFsm)
        {
            _stoppingDistance = stoppingDistance;
            _moveSpeed = moveSpeed;
            _roamTime = roamTime;
            _myTransform = myTransform;
            _rotateSpeed = rotateSpeed;
            _roamPoints = roamPoints;
            _playerTransform = playerTransform;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter roam");
            _currentRoamTime = _roamTime;
        }

        public override void Update()
        {
            if (_currentRoamTime > _roamTime)
                FinishState();
            else
            {
                _currentRoamTime += Time.deltaTime;
                LookAtPlayer();
                MoveRightAndLeft();
            }
        }

        private void MoveRightAndLeft()
        {
            if (_pointIndex <= _roamPoints.Count - 1)
            {
                if (Vector3.Distance(_myTransform.position, _roamPoints[_pointIndex].transform.position) >
                    _stoppingDistance)
                {
                    var nextPoint = _roamPoints[_pointIndex].transform.position;
                    _myTransform.position =
                        Vector3.MoveTowards(_myTransform.position, nextPoint, Time.deltaTime * _moveSpeed);
                }
                else
                {
                    _pointIndex++;
                    if (_pointIndex >= _roamPoints.Count)
                        _pointIndex = 0;
                }
            }
        }

        private void LookAtPlayer()
        {
            var dir = _playerTransform.position - _myTransform.position;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            _myTransform.rotation =
                Quaternion.RotateTowards(_myTransform.rotation, rot, _rotateSpeed * Time.deltaTime);
        }
    }
}