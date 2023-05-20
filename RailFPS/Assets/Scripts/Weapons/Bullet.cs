using System;
using Interfaces;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        private float _damage;
        private float _speed;
        private Vector3 _dir;
        private Rigidbody _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
        }

        public void Init(Vector3 dir,float damage, float speed)
        {
            _dir = dir;
            _damage = damage;
            _speed = speed;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _dir, Time.deltaTime * _speed);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.TryGetComponent(out IDamagable damagable) && damagable.CanBeDamaged)
                damagable.GetDamaged(_damage);
            
            Destroy(this.gameObject);
        }
    }
}