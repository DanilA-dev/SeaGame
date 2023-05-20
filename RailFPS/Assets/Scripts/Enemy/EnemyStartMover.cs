using System;
using DG.Tweening;
using UnityEngine;

namespace Enemy
{
    public class EnemyStartMover : MonoBehaviour
    {
        [SerializeField] private float _moveTime;
        [SerializeField] private Transform _startMovePoint;

        private void OnEnable()
        {
            transform.DOMove(_startMovePoint.position, _moveTime).SetAutoKill(gameObject);
        }

        private void OnDrawGizmos()
        {
            if(null == _startMovePoint)
                return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _startMovePoint.position);
            Gizmos.DrawSphere(_startMovePoint.position, 1f);
        }
    }

}
