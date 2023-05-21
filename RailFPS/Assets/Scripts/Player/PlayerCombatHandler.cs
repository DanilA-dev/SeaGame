using System.Collections.Generic;
using DG.Tweening;
using Interfaces;
using Sirenix.OdinInspector;
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
        [Header("Shake Settings")]
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private float _shakeTime;
        [SerializeField] private float _shakeStrength;
        [SerializeField,ReadOnly]private bool _isReloading;
        [Header("Particles")]
        [SerializeField] private ParticleSystem _muzzleParticle;

        [Header("Sounds")]
        [SerializeField] private List<AudioClip> _gunShotsClips;
        [SerializeField] private AudioClip _reloadClip;

        private Animator _animator;
        private Camera _cam;
        private Vector3 _mousePoint;
        private float _nextTimeToFire;
        private float _currentReloadTime;
        private int _currentAmmoCount;
        private AudioSource _audioSource;

        private readonly int _reloadHash = Animator.StringToHash("Reload");

        public void Init(Animator animator)
        {
            _cam = Camera.main;
            _audioSource = GetComponent<AudioSource>();
            _animator = animator;
            _currentAmmoCount = _ammoCount;
            _currentReloadTime = _reloadTime;
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(0) && Time.time >= _nextTimeToFire)
            {
                Shoot();
            }
            
            if(Input.GetMouseButtonUp(0))
                _muzzleParticle.Stop();

            if (_currentAmmoCount <= 0)
            {
                if (!_isReloading)
                {
                    _isReloading = true;
                    PlayReloadSound();
                    _animator.SetTrigger(_reloadHash);
                }
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
                    _muzzleParticle.Play();
                    damagable.GetDamaged(_damage);
                }
            }

            PlayRandomShotSound();
            _gunPoint.DOShakePosition(_shakeTime, _shakeStrength);
            _nextTimeToFire = Time.time + 1f / _rateOfFire;
            _currentAmmoCount--;
               
        }
        
        public Vector3 GetRandomSpread()
        {
            return new Vector3(Random.Range(_minSpread.x, _maxSpread.x),
                Random.Range(_minSpread.y, _maxSpread.y), Random.Range(_minSpread.z, _maxSpread.z));
        }

        private void PlayRandomShotSound()
        {
            int index = Random.Range(0, _gunShotsClips.Count);
            _audioSource.PlayOneShot(_gunShotsClips[index]);
        }

        private void PlayReloadSound()
        {
            _audioSource.PlayOneShot(_reloadClip);
        }
    }
}