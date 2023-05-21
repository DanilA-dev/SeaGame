using System;
using System.Collections.Generic;
using DG.Tweening;
using Interfaces;
using Misc;
using UnityEngine;
using StateMachine;
using StateMachine.EnemyStates;

namespace Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, IDamagable
    {
        [Header("On Damage")]
        [SerializeField] private float _shakeStregth;
        [SerializeField] private float _shakeTime;
        [SerializeField] private ParticleSystem _damageParticle;
        [Header("Stats")]
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected float _health;
        [SerializeField] protected float _attackDamage;
        [SerializeField] protected float _rotateSpeed;
        [SerializeField] protected float _stopDistance;
        [Header("AppearState")]
        [SerializeField] private List<EnemyMovePoint> _appearPoints;
        [SerializeField] private float _additionApeearSpeed;
        [Header("RoamState")]
        [SerializeField] private float _roamTime;
        [SerializeField] private List<EnemyMovePoint> _roamPoints;
        [SerializeField] private AudioClip _dmgClip;
        [SerializeField] private AudioClip _deathClip;
        
         private AudioSource _audioSource;

        protected Transform _player;
        private float _currentHealth;
        
        protected SimpleFSM _simpleFsm;
        protected EnemyScreenAppearState _startState;
        protected ScreenRoamState _roamState;
        
        public event Action<BaseEnemy> OnEnemyKilled;
        public bool CanBeDamaged => true;

        public void Init(Transform player)
        {
            _player = player;
            _currentHealth = _health;
            _audioSource = GetComponent<AudioSource>();
            _simpleFsm = new SimpleFSM();

            _startState = new EnemyScreenAppearState(_simpleFsm, _moveSpeed, transform,
                _stopDistance, _rotateSpeed, _appearPoints, _additionApeearSpeed);
            
            _roamState = new ScreenRoamState(_simpleFsm, transform,_player,_rotateSpeed, _moveSpeed,
                _roamTime,_stopDistance, _roamPoints);
            
            _simpleFsm.AddState(_startState);
            _simpleFsm.AddState(_roamState);
            _simpleFsm.ChangeState(_startState);

            _startState.OnStateExit += () => _simpleFsm.ChangeState(_roamState);
        }
        

        protected virtual void OnDestroy()
        {
            _startState.OnStateExit -= () => _simpleFsm.ChangeState(_roamState);
        }

        private void Update()
        {
            _simpleFsm.UpdateState();
        }


        public void GetDamaged(float amount)
        {
            _currentHealth -= amount;
            transform.DOShakeScale(_shakeTime, _shakeStregth);
            _audioSource.PlayOneShot(_dmgClip);
            if(_currentHealth <= 0)
                Die();
        }

        protected virtual void Die()
        {
            OnEnemyKilled?.Invoke(this);
            _damageParticle.Play();
            _audioSource.PlayOneShot(_deathClip);
            gameObject.SetActive(false);
        }
        
    }

}
