namespace Interfaces
{
    public interface IDamagable
    {
        public bool CanBeDamaged { get; }
        public void GetDamaged(float amount);
    }
}