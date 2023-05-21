
namespace Enemy
{
    public class BlowFish : BaseEnemy
    {
        protected  void Start()
        {
            _roamState.OnStateExit += () => _simpleFsm.ChangeState(_roamState);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _roamState.OnStateExit -= () => _simpleFsm.ChangeState(_roamState);
        }
    }
}