using UnityEngine;

namespace Weapons
{
    public class RaycastRangeGun : BaseWeapon
    {
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _rateOfFire;
        [SerializeField] private Vector3 _minSpread;
        [SerializeField] private Vector3 _maxSpread;
        [SerializeField,Min(1)] private int _ammoCount;
        
        
    }
}