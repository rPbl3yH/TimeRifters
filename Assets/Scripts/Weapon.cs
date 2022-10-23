using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    ShootGun,
    Rifle
}

public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField] public int Damage { get; protected set; }
    [field: SerializeField] public int StartDamage { get; protected set; }
    [field: SerializeField] public float DelayBetweenShoot { get; protected set; }
    [field: SerializeField] public float StartDelayBetweenShoot { get; protected set; }
    [field: SerializeField] public AudioClip ShootSound { get; private set; }

    [field: SerializeField] public WeaponType WeaponType { get; protected set; }

    [SerializeField] private Transform _bulletSpawn;
    
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GameObject _muzzleEffect;
    [SerializeField] private float _damageMultiplier;
    [SerializeField] private float _delayMultiplier;

    public Aim Aim { get; set; }
    protected float Timer;
    private GameObject _aim;
    public bool IsShoot;

    public bool IsPoison { get; protected set; }
    public bool IsUI;

    protected bool IsInitialized;

    public void Initialize(Weapon weapon) {

        Damage = weapon.Damage;
        DelayBetweenShoot = weapon.DelayBetweenShoot;
        IsPoison = weapon.IsPoison;
        WeaponType = weapon.WeaponType;
        IsInitialized = true;
    }

    public void Initialize(WeaponSetting setting) {
        if (IsInitialized) {
            print("Weapon has already initialized");
            return;
        }
        Damage = setting.DefaultDamage;
        DelayBetweenShoot = setting.DefaultDelay;
        IsPoison = setting.DefaultPoison;
        WeaponType = setting.WeaponType;
        _hitEffect = setting.HitEffect;
        _muzzleEffect = setting.MuzzleEffect;
        ShootSound = setting.ShootSound;
        _damageMultiplier = setting.DamageMultiplier;
        _delayMultiplier = setting.DelayMultiplier;
        IsInitialized = true;
    }

    private void Start() {
        if (IsUI) {
            enabled = false;
        } 
    }

    private void FixedUpdate() {
        IsShoot = false;
    }

    public virtual void Update() {

        Timer += Time.deltaTime;
        if (Timer > DelayBetweenShoot) {
            if (Input.GetMouseButtonDown(0)) {
                Shoot();
                Timer = 0;
            }
            else if (Input.GetMouseButton(0)) {
                Shoot();
                Timer = 0;
            }
        }
        

        if(Aim)
            SetAimPosition();
    }

    private void SetAimPosition() {
        Ray ray = new Ray(_bulletSpawn.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.TryGetComponent(out IDamageable damageable)) {
                
                Aim.SetAimPosition(hit, _bulletSpawn.position);
            }
        }

    }

    public virtual void Shoot() {
        CreateMuzzleEffect();
        IsShoot = true;
        Ray ray = new Ray(_bulletSpawn.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.TryGetComponent(out IDamageable damageable)) {
                if (IsPoison) {
                    if (hit.collider.TryGetComponent(out IPoisonable poisonable)) {
                        poisonable.StartPoisonDamage(Damage);
                    }
                }
                damageable.TakeDamage(Damage);

                GameObject hitEffect = Instantiate(_hitEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.collider.transform);
                Destroy(hitEffect, 1f);
            }
        }

        GameManager.Instance.EventManager.Shooted(this);
    }

    private void CreateMuzzleEffect() {
        GameObject muzzleEffect = Instantiate(_muzzleEffect, _bulletSpawn.position, Quaternion.identity);
        Destroy(muzzleEffect, 0.3f);
    }

    public void ImproveDamage() {
        //print("old damage " + Damage);
        var damage = Damage * _damageMultiplier;
        Damage = (int)damage;
        //print("new damage " + Damage);
    }

    public int GetPreviewUpgradeDamage() {
        var damage = Damage * _damageMultiplier;
        return (int)damage;
    }

    public void ImproveDelay() {
        //print("old delay " + DelayBetweenShoot);
        DelayBetweenShoot /= _delayMultiplier;
        //print("new delay " + DelayBetweenShoot);
    }
    public float GetPreviewUpgradeDelay() {
        float delay = DelayBetweenShoot / _delayMultiplier; ;
        return delay;
    }

    public void ImprovePoison() {
        if (!IsPoison)
            IsPoison = true;
    }

    public bool GetPreviewUgradePoison() => true;

    public virtual void PrintInfo() {
        print($"Weapon {this} Damage {Damage} Delay {DelayBetweenShoot} Poison {IsPoison}");
    }
}
