using Interfaces;
using Player.Combat;
using StageSystem;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerHealSignal
    {
        public float HealAmount { get; private set; }
        public PlayerHealSignal(float healAmount)
        {
            HealAmount = healAmount;
        }
    }
    
    public class PlayerHealthChangeSignal
    {
        public bool IsDamaged { get; private set; }
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }

        public PlayerHealthChangeSignal(float currentHealth, float maxHealth, bool isDamaged)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            IsDamaged = isDamaged;
        }
    }
    
    public class PlayerBase : MonoBehaviour, IDamagable
    {
        [Header("Main Components")]
        [SerializeField] private PlayerRailMovement _playerRailMovement;
        [SerializeField] private PlayerCombatHandler _combatHandler;
        [Space]
        [SerializeField] private float _health;
        [SerializeField] private Animator _animator;
        [Space]
        [SerializeField] private UnityEvent OnDamageEvent;

        private float _currentHealth;
        
        public bool CanBeDamaged => true;
        private void Awake()
        {
            _currentHealth = _health;
            _playerRailMovement.Init(_animator, _combatHandler);
            _combatHandler.Init(_animator);

            MessageBroker.Default.Receive<PlayerHealSignal>()
                .Subscribe(signal => HealPlayer(signal.HealAmount)).AddTo(gameObject);
        }

        private void HealPlayer(float signalHealAmount)
        {
            _currentHealth += signalHealAmount;
            MessageBroker.Default.Publish(new PlayerHealthChangeSignal(_currentHealth, _health, false));
        }

        public void GetDamaged(float amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0)
                PlayerDie();
            
            MessageBroker.Default.Publish(new PlayerHealthChangeSignal(_currentHealth, _health, true));
            OnDamageEvent?.Invoke();
        }

        private void PlayerDie()
        {
            MessageBroker.Default.Publish(new GameStateSignal(GameState.Lose));
        }
    }
}