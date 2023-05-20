using System.Collections.Generic;
using Player;
using UniRx;
using UnityEngine;

namespace StageSystem
{
    public class StageSystem : MonoBehaviour
    {
        [SerializeField] private PlayerRailMovement _playerRailMovement;
        [SerializeField] private List<RailStage> _stages;

        private void Awake()
        {
            MessageBroker.Default.Receive<RaillReachSignal>()
                .Subscribe(signal => UpdateActiveStage(signal.Index)).AddTo(gameObject);
        }

        private void Start()
        {
            DisableAllStages();
            SubscribeToStageClear();
            _playerRailMovement.MoveToNextPoint();
        }

        private void OnDisable()
        {
            UnSubscribeFromStageClear();
        }

        private void SubscribeToStageClear()
        {
            foreach (var stage in _stages)
                stage.OnStageClear += () => _playerRailMovement.MoveToNextPoint();
        }
        
        private void UnSubscribeFromStageClear()
        {
            foreach (var stage in _stages)
                stage.OnStageClear -= () => _playerRailMovement.MoveToNextPoint();
        }

        private void UpdateActiveStage(int railIndex)
        {
            foreach (var stage in _stages)
            {
                if(stage.Index == railIndex)
                    stage.gameObject.SetActive(true);
            }
        }
        
        private void DisableAllStages()
        {
            if(_stages.Count >= 0)
                _stages.ForEach(s => s.gameObject.SetActive(false));
        }
    }
}