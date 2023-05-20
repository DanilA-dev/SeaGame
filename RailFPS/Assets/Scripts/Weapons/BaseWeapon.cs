using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons
{
    public enum WeaponStatus
    {
        Serenity = 0,
        Using = 1,
        Reloading = 2
    }
    
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected WeaponStatus _status;
        [SerializeField, Min(0.1f)] protected float _damage;
        [SerializeField, Range(0, 20)] protected float _reloadTime;

        public virtual void Attack(Vector3 attackDir) => _status = WeaponStatus.Using;

        public void Reload()
        {
            StartCoroutine(ReloadRoutine(_reloadTime));
        }

        protected virtual IEnumerator ReloadRoutine(float reloadTime)
        {
            if (_status != WeaponStatus.Reloading)
            {
                _status = WeaponStatus.Reloading;
                yield return new WaitForSeconds(reloadTime);
                _status = WeaponStatus.Serenity;
            }
        }
    }

}
