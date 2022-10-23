
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Settings")]
public class WeaponSettings : ScriptableObject
{
    public WeaponSetting[] WeaponsSettings;
}

[CreateAssetMenu(menuName = "Weapon/Weapon Setting")]
public class WeaponSetting : ScriptableObject {
    public WeaponType WeaponType;

    public int DefaultDamage;
    public float DefaultDelay;
    public bool DefaultPoison;
    public AudioClip ShootSound;
    public GameObject HitEffect;
    public GameObject MuzzleEffect;
    public float DamageMultiplier;
    public float DelayMultiplier;
}

