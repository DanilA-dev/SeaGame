using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using StageSystem;
using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerRailMovement : MonoBehaviour
    {
        [SerializeField] private float _movingPointTime;
        [SerializeField] private float _rotationPointTime;
        [SerializeField] private List<RaillPoint> _movePoints = new List<RaillPoint>();

        private Animator _animator;
        private Transform _myTransform;
        private int _pointIndex;

        private readonly int _moveHash = Animator.StringToHash("Move");

        public void Init(Animator animator)
        {
            _myTransform = transform;
            _animator = animator;

            MessageBroker.Default.Receive<StageChangeSingal>()
                .Subscribe(signal => MoveToNextPoint()).AddTo(gameObject);
        }
        
        private void MoveToNextPoint()
        {
            if(_movePoints.Count <= 0)
                return;

            if(_pointIndex > _movePoints.Count - 1)
                return;
            
            var nextPoint = _movePoints[_pointIndex];
            _animator?.SetBool(_moveHash, true);
            var seq = DOTween.Sequence();
            seq.Append(_myTransform.DOMove(nextPoint.transform.position, _movingPointTime));
            seq.Join(_myTransform.DORotate(nextPoint.transform.eulerAngles, _rotationPointTime));
            seq.OnComplete(() =>
            {
                nextPoint.ReachPoint();
                _animator?.SetBool(_moveHash, false);
                _pointIndex++;
            });
        }

        [Button]
        private void NextPointViewDebug()
        {
            transform.position = _movePoints[_pointIndex].transform.position;
            transform.rotation = _movePoints[_pointIndex].transform.rotation;
            _pointIndex++;
            if (_pointIndex >= _movePoints.Count)
                _pointIndex = 0;
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
