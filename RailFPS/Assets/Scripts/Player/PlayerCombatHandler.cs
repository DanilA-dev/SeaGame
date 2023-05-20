using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerCombatHandler : MonoBehaviour
    {
        [Header("GunSettings")] 
        [SerializeField] private float _damage;
        [SerializeField] private float _range;
        [SerializeField] private float _rateOfFire;
        [SerializeField] private float _reloadTime;
        [SerializeField] private Vector3 _minSpread;
        [SerializeField] private Vector3 _maxSpread;
        [SerializeField,Min(1)] private int _ammoCount;
        
        private Camera _cam;
        private Vector3 _mousePoint;
        private float _nextTimeToFire;
        private float _currentReloadTime;
        private bool _isReloading;
        private int _currentAmmoCount;
        

        private void Awake()
        {
            _cam = Camera.main;
            _currentAmmoCount = _ammoCount;
            _currentReloadTime = _reloadTime;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && Time.time >= _nextTimeToFire)
            {
                Shoot();
            }

            if (_currentAmmoCount <= 0)
            {
                _isReloading = true;
                if (_currentReloadTime > 0)
                    _currentReloadTime -= Time.deltaTime;
                else
                {
                    _isReloading = false;
                    _currentAmmoCount = _ammoCount;
                    _currentReloadTime = _reloadTime;
                }
            }
        }

        private void Shoot()
        {
            if(_isReloading)
                return;
            
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction + GetRandomSpread(), out RaycastHit hit, _range))
            {
                if (hit.collider.TryGetComponent(out IDamagable damagable) && damagable.CanBeDamaged)
                {
                    Debug.Log(damagable);
                    damagable.GetDamaged(_damage);
                }
            }

            _nextTimeToFire = Time.time + 1f / _rateOfFire;
            _currentAmmoCount--;
               
        }
        
        public Vector3 GetRandomSpread()
        {
            return new Vector3(Random.Range(_minSpread.x, _maxSpread.x),
                Random.Range(_minSpread.y, _maxSpread.y), Random.Range(_minSpread.z, _maxSpread.z));
        }
    }
}