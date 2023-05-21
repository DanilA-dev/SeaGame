using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class EnemyScreenAppearState : BaseState
    {
        private readonly float _additionAppearSpeed;
        private readonly float _stopDistance; 
        private readonly float _rotateSpeed;
        private readonly List<EnemyMovePoint> _appearMovePoints;

        private readonly Transform _myTransform;
        private readonly float _moveSpeed;

        private int _appearMoveIndex;

        public EnemyScreenAppearState(SimpleFSM simpleFsm, float moveSpeed, Transform myTransform, float stopDistance,
            float rotateSpeed, List<EnemyMovePoint> appearMovePoints, float additionAppearSpeed) : base(
            simpleFsm)
        {
            _moveSpeed = moveSpeed;
            _myTransform = myTransform;
            _stopDistance = stopDistance;
            _rotateSpeed = rotateSpeed;
            _additionAppearSpeed = additionAppearSpeed;
            _appearMovePoints = appearMovePoints;
        }

        public override void Update()
        {
            if (_appearMoveIndex <= _appearMovePoints.Count - 1)
            {
                if (Vector3.Distance(_myTransform.position, _appearMovePoints[_appearMoveIndex].transform.position) >
                    _stopDistance)
                {
                    var nextPoint = _appearMovePoints[_appearMoveIndex].transform.position;
                    var dir = nextPoint - _myTransform.position;
                    _myTransform.position =
                        Vector3.MoveTowards(_myTransform.position, nextPoint,
                            Time.deltaTime * (_moveSpeed + _additionAppearSpeed));

                    Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
                    _myTransform.rotation =
                        Quaternion.RotateTowards(_myTransform.rotation, rot, _rotateSpeed * Time.deltaTime);
                }
                else
                {
                    _appearMoveIndex++;
                    if(_appearMoveIndex >= _appearMovePoints.Count)
                       FinishState();
                }
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit apeear state");
            _appearMoveIndex = 0;
        }

        public override void DrawGizmo()
        {
            if(_appearMovePoints.Count <= 0)
                return;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < _appearMovePoints.Count; i++)
            {
                Gizmos.DrawSphere(_appearMovePoints[i].transform.position, 1f);
                if(i + 1< _appearMovePoints.Count)
                    Gizmos.DrawLine(_appearMovePoints[i].transform.position, _appearMovePoints[i + 1].transform.position);
            }
        }
    }
}