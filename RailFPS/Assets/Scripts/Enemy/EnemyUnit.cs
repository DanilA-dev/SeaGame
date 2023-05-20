using System;
using Interfaces;
using UnityEngine;
using Weapons;

namespace Enemy
{
    public class EnemyUnit : MonoBehaviour, IDamagable
    {
        [SerializeField] private float _health;
        [SerializeField] private WeaponHandler _weaponHandler;
        [SerializeField] private Transform _playerTransform;

        private float _currentHealth;

        private void Start()
        {
            _currentHealth = _health;
        }

        private void Update()
        {
            var dirToPlayer = (_playerTransform.position - transform.position).normalized;
            Quaternion desiredRot = Quaternion.LookRotation(Vector3.up, dirToPlayer);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot,500);
        }

        public bool CanBeDamaged => true;
        public void GetDamaged(float amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        {
           Destroy(this.gameObject);
        }
    }
}