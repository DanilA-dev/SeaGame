using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class ProjecttileRangeGun : BaseWeapon
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _rateOfFire;
        [SerializeField] private Vector3 _minSpread;
        [SerializeField] private Vector3 _maxSpread;
        [SerializeField,Min(1)] private int _ammoCount;

        private float _nextTimeToFire;
        private int _currentAmmoCount;

        private void Start()
        {
            _currentAmmoCount = _ammoCount;
        }

        public override void Attack(Vector3 attackDir)
        {
            if (CanShoot())
            {
                if(_currentAmmoCount <= 0)
                    Reload();

                base.Attack(attackDir);
                var newBullet = Instantiate(_bulletPrefab);
                newBullet.transform.position = _bulletSpawnPoint.position;
                newBullet.Init(attackDir + GetRandomSpread(), _damage, _bulletSpeed);
                _currentAmmoCount--;
                _nextTimeToFire = Time.time + 1f / _rateOfFire;
            }
        }

        protected override IEnumerator ReloadRoutine(float reloadTime)
        {
            if (_status != WeaponStatus.Reloading)
            {
                _status = WeaponStatus.Reloading;
                yield return new WaitForSeconds(reloadTime);
                _status = WeaponStatus.Serenity;
                _currentAmmoCount = _ammoCount;
            }
        }


        private bool CanShoot() => _status != WeaponStatus.Reloading && Time.time >= _nextTimeToFire;

        public Vector3 GetRandomSpread()
        {
            return new Vector3(Random.Range(_minSpread.x, _maxSpread.x),
                Random.Range(_minSpread.y, _maxSpread.y), Random.Range(_minSpread.z, _maxSpread.z));
        }
    }
}