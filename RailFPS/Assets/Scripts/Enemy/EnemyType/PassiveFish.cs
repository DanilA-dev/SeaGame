
using Player;
using UniRx;
using UnityEngine;

namespace Enemy
{
   
    public class PassiveFish : BaseEnemy
    {
        [SerializeField] private float _healthOnDie;
        
        protected  void Start()
        {
            _roamState.OnStateExit += () => _simpleFsm.ChangeState(_roamState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _roamState.OnStateExit -= () => _simpleFsm.ChangeState(_roamState);
        }

        protected override void Die()
        {
            MessageBroker.Default.Publish(new PlayerHealSignal(_healthOnDie));
            base.Die();
        }
    }
}