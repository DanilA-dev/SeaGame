using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace StageSystem
{
    public class RailStage : MonoBehaviour
    {
        [SerializeField] private int _index;
        [SerializeField] private bool _isClear;
        [SerializeField] private List<EnemyUnit> _enemyUnits;

        public event Action OnStageClear;
                
        public int Index => _index;

        private void Awake()
        {
            foreach (var enemy in _enemyUnits)
                enemy.OnEnemyKilled += UpdateEnemyList;
        }

        private void OnDestroy()
        {
            foreach (var enemy in _enemyUnits)
                enemy.OnEnemyKilled -= UpdateEnemyList;
        }

        private void UpdateEnemyList(EnemyUnit killedEnemy)
        {
            if (_enemyUnits.Contains(killedEnemy))
                _enemyUnits.Remove(killedEnemy);
            
            if(_enemyUnits.Count == 0)
                OnStageClear?.Invoke();
        }
    }
}