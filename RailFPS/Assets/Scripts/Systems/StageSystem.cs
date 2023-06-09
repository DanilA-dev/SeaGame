﻿using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace StageSystem
{
    public class StageChangeSingal
    {
        public int StageIndex { get; private set; }

        public StageChangeSingal(int stageIndex)
        {
            StageIndex = stageIndex;
        }
    }
    
    public class StageSystem : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private List<RailStage> _stages;

        private void Awake()
        {
            if(_stages.Count > 0)
                _stages.ForEach(s => s.Init(_player));
            
            MessageBroker.Default.Receive<RaillReachSignal>()
                .Subscribe(signal => UpdateActiveStage(signal.Index)).AddTo(gameObject);
        }

        private void Start()
        {
            DisableAllStages();
            SubscribeToStageClear();
            MessageBroker.Default.Publish(new StageChangeSingal(0));
        }

        private void OnDisable()
        {
            UnSubscribeFromStageClear();
        }

        private void SubscribeToStageClear()
        {
            foreach (var stage in _stages)
                stage.OnStageClear += (index) => MessageBroker.Default.Publish(new StageChangeSingal(index));
        }
        
        private void UnSubscribeFromStageClear()
        {
            foreach (var stage in _stages)
                stage.OnStageClear -= (index) => MessageBroker.Default.Publish(new StageChangeSingal(index));
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