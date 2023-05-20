using UnityEngine;
using Weapons;

namespace Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private BaseWeapon _currentWeapon;

        public void Attack(Vector3 attackDir)
        {
            _currentWeapon?.Attack(attackDir);
        }
    }
}