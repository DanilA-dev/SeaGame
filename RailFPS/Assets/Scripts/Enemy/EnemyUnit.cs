using System;
using Interfaces;
using UnityEngine;

namespace Enemy
{
    public class EnemyUnit : MonoBehaviour, IDamagable
    {
        [SerializeField] private float _health;
        [SerializeField] private Transform _playerTransform;

        public event Action<EnemyUnit> OnEnemyKilled;
        
        private float _currentHealth;
        private int _moveIndex;

        public bool CanBeDamaged => true;
        private void Start()
        {
            _currentHealth = _health;
        }

        public void GetDamaged(float amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            OnEnemyKilled?.Invoke(this);
           Destroy(this.gameObject);
        }
    }
}