using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UpgradeType
{
    Damage,
    Delay,
    Poison
}

public class Upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Weapon CurrentWeapon { private get; set; }
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private string _beginningTitle, _endingTitle;
    [SerializeField] private TMP_Text _upgradeText;
    [SerializeField] private Color _colorPreview;

    private Color _beginnigColor;

    private void Start() {
        _beginnigColor = _upgradeText.color;
        
    }

    public void ResetValues() {
        if (CurrentWeapon == null) return;
        foreach (var weaponSetting in GameManager.Instance.WeaponSettings.WeaponsSettings) {
            if (weaponSetting.WeaponType == CurrentWeapon.WeaponType) {    
                switch (_upgradeType) {
                    case UpgradeType.Damage:
                        UpdateText(weaponSetting.DefaultDamage);
                        break;
                    case UpgradeType.Delay:
                        UpdateText(weaponSetting.DefaultDelay);
                        break;
                    case UpgradeType.Poison:
                        UpdateText(weaponSetting.DefaultPoison);
                        break;
                }
            }
        }
    }

    private void UpdatePreviewValue() {
        switch (_upgradeType) {
            case UpgradeType.Damage:
                UpdateText(CurrentWeapon.GetPreviewUpgradeDamage());
                break;
            case UpgradeType.Delay:
                UpdateText(CurrentWeapon.GetPreviewUpgradeDelay());
                break;
            case UpgradeType.Poison:
                UpdateText(CurrentWeapon.GetPreviewUgradePoison());
                break;
        }
    }

    public void UpdateValue() {
        switch (_upgradeType) {
            case UpgradeType.Damage:
                print(CurrentWeapon.Damage);
                UpdateText(CurrentWeapon.Damage);
                break;
            case UpgradeType.Delay:
                print(CurrentWeapon.DelayBetweenShoot);
                UpdateText(CurrentWeapon.DelayBetweenShoot);
                break;
            case UpgradeType.Poison:
                print(CurrentWeapon.IsPoison);
                UpdateText(CurrentWeapon.IsPoison);
                break;
        }
    }

    private void UpdateDefaultValue() {
        switch (_upgradeType) {
            case UpgradeType.Damage:
                UpdateText(CurrentWeapon.Damage);
                break;
            case UpgradeType.Delay:
                UpdateText(CurrentWeapon.DelayBetweenShoot);
                break;
            case UpgradeType.Poison:
                UpdateText(CurrentWeapon.IsPoison);
                break;
        }
    }

    public void SetWeaponValue(Weapon weapon) {
        CurrentWeapon = weapon;
    }

    private void UpdateText(int value) {
        _upgradeText.text = $"{_beginningTitle} {value} {_endingTitle}";
    }

    private void UpdateText(float value) {
        _upgradeText.text = $"{_beginningTitle} {value.ToString("0.0")} {_endingTitle}";
    }

    private void UpdateText(bool value) {
        var result = value ? "On" : "Off";
        _upgradeText.text = $"{_beginningTitle} {result} {_endingTitle}";
    }

    public void OnPointerEnter(PointerEventData eventData) {
        UpdatePreviewValue();
        _upgradeText.color = _colorPreview;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _upgradeText.color = _beginnigColor;
        UpdateDefaultValue();
    }
}
