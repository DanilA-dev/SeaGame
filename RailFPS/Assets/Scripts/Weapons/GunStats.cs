using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns
{
    [CreateAssetMenu(menuName = "Data/Gun")]
    public class GunStats : ScriptableObject
    {
        [field:SerializeField] public float Damage { get; private set; }
        [field:SerializeField] public float Range { get; private set; }
        [field:SerializeField] public float RateOfFire { get; private set; }
        [field:SerializeField] public float ReloadTime { get; private set; }
        [field:SerializeField] public int AmmoCapactiy { get; private set; }
    }
}