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
        [SerializeField] private List<BaseEnemy> _enemyUnits;

        public event Action<int> OnStageClear;
                
        public int Index => _index;

        public void Init(Transform player)
        {
            if(_enemyUnits.Count > 0)
                _enemyUnits.ForEach(e => e.Init(player));
            
            foreach (var enemy in _enemyUnits)
                enemy.OnEnemyKilled += UpdateEnemyList;
        }
        
        private void OnDestroy()
        {
            foreach (var enemy in _enemyUnits)
                enemy.OnEnemyKilled -= UpdateEnemyList;
        }

        private void UpdateEnemyList(BaseEnemy killedEnemy)
        {
            if (_enemyUnits.Contains(killedEnemy))
                _enemyUnits.Remove(killedEnemy);

            if (_enemyUnits.Count == 0)
            {
                _isClear = true;
                OnStageClear?.Invoke(_index);
            }
        }
    }
}