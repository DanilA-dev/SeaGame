using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using StageSystem;
using UnityEngine;

namespace Player
{
    public class PlayerRailMovement : MonoBehaviour
    {
        [SerializeField] private float _movingPointTime;
        [SerializeField] private float _rotationPointTime;
        [SerializeField] private List<RaillPoint> _movePoints = new List<RaillPoint>();

        private Transform _myTransform;
        private int _pointIndex;

        private void Awake()
        {
            _myTransform = transform;
        }
        
        public void MoveToNextPoint()
        {
            if(_movePoints.Count <= 0)
                return;

            if(_pointIndex > _movePoints.Count)
                return;
            
            var nextPoint = _movePoints[_pointIndex];
            var seq = DOTween.Sequence();
            seq.Append(_myTransform.DOMove(nextPoint.transform.position, _movingPointTime));
            seq.Join(_myTransform.DORotate(nextPoint.transform.eulerAngles, _rotationPointTime));
            seq.OnComplete(() =>
            {
                nextPoint.ReachPoint();
                _pointIndex++;
            });
        }

        private void OnDrawGizmos()
        {
            if(_movePoints.Count <= 0)
                return;

            Gizmos.color = Color.red;
            for (int i = 0; i < _movePoints.Count; i++)
            {
                Gizmos.DrawSphere(_movePoints[i].transform.position, 1f);
                if(i + 1< _movePoints.Count)
                    Gizmos.DrawLine(_movePoints[i].transform.position, _movePoints[i + 1].transform.position);
            }
        }
    }

}
