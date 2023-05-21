using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerBase : MonoBehaviour, IDamagable
    {
        [Header("Main Components")]
        [SerializeField] private PlayerRailMovement _playerRailMovement;
        [SerializeField] private PlayerCombatHandler _combatHandler;
        [Space] 
        [SerializeField] private Animator _animator;
        [Space]
        [SerializeField] private UnityEvent OnDamageEvent;

        public bool CanBeDamaged => true;
        private void Awake()
        {
            _playerRailMovement.Init(_animator);
            _combatHandler.Init(_animator);
        }

        public void GetDamaged(float amount)
        {
            Debug.Log("Player damaged");
            OnDamageEvent?.Invoke();
        }
    }
}