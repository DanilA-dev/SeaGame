using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerRailMovement : MonoBehaviour
    {
        [SerializeField] private float _movingPointTime;
        [SerializeField] private float _rotationPointTime;
        [SerializeField] private List<Transform> _movePoints = new List<Transform>();

        private Transform _myTransform;
        private int _pointIndex;

        private void Awake()
        {
            _myTransform = transform;
        }

        
        [Button]
        private void MoveToNextPoint()
        {
            if(_movePoints.Count <= 0)
                return;

            _pointIndex++;
            if(_pointIndex >= _movePoints.Count)
                return;

            var nextPoint = _movePoints[_pointIndex];
            var seq = DOTween.Sequence();
            seq.Append(_myTransform.DOMove(nextPoint.position, _movingPointTime));
            seq.Join(_myTransform.DORotate(nextPoint.eulerAngles, _rotationPointTime));
        }
    }

}
