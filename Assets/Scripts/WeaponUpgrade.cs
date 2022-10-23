using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WeaponUpgrade : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentCostText, _nextCostText, _titleText, _moneyText;
    [SerializeField] private float _multiplierToCost;
    [SerializeField] private int _startCost = 100;
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private Upgrade[] _upgrades;
    [SerializeField] private GameObject _weaponChooseMenu;
    private Weapon _currentWeapon;

    private int _currentCost;
    private int _nextCost;

    private void Start() {
        InitializeAllWeapons();
        SetStartCosts();
        UpdateTexts();

        GameManager.Instance.EventManager.OnChoseWeapon += OnChoseWeapon;
        GameManager.Instance.EventManager.OnRoundFinished += OnRoundFinished;

        foreach (var weapon in _weapons) {
            weapon.gameObject.SetActive(false);
        }

        _weaponChooseMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnRoundFinished() {
        foreach (var weapon in _weapons) {
            weapon.gameObject.SetActive(false);
        }
        InitializeAllWeapons();
        //_moneyText.text = "Coins " + CoinManager.Coins;
    }

    private void InitializeAllWeapons() {
        foreach (var weapon in _weapons) {
            foreach (var setting in GameManager.Instance.WeaponSettings.WeaponsSettings) {
                if (setting.WeaponType == weapon.WeaponType) {
                    weapon.Initialize(setting);
                }
            }
        }
    }

    private void SetStartCosts() {
        _currentCost = _startCost;
        _nextCost = (int)(_currentCost * _multiplierToCost);

    }

    private void OnChoseWeapon(Weapon weapon) {
        _weaponChooseMenu.SetActive(false);
        SetCurrentWeapon(weapon);
    }

    private void SetCurrentWeapon(Weapon value) {
        foreach (var weapon in _weapons) {
            if (weapon.GetType().Equals(value.GetType())) {
                _currentWeapon = weapon;
                weapon.gameObject.SetActive(true);
                //print("current weapon " + _currentWeapon);
                break;
            }
            else {
                weapon.gameObject.SetActive(false);
            }
        }
        SetWeaponToUpgrades();
        UpdateTexts();
    }

    private void UpdateCosts() {
        _currentCost = _nextCost;
        _nextCost = (int)(_nextCost * _multiplierToCost);
        UpdateTexts();
    }

    private void UpdateTexts() {
        _currentCostText.text = "Cost: " + _currentCost;
        _nextCostText.text = "Next cost: " + _nextCost;
        if (_currentWeapon)
            _titleText.text = _currentWeapon.name;
        _moneyText.text = "Coins: " + CoinManager.Coins;
        
    }

    public void BuyImproveDamage(Upgrade upgrade) {
        if (CoinManager.Coins >= _currentCost) {
            GameManager.Instance.CoinManager.ReduceCoin(_currentCost);
            BuyUpgrade(UpgradeType.Damage);
            upgrade.UpdateValue();
            UpdateCosts();
        }
    }

    public void BuyImproveDelay(Upgrade upgrade) {
        if (CoinManager.Coins >= _currentCost) {
            GameManager.Instance.CoinManager.ReduceCoin(_currentCost);
            BuyUpgrade(UpgradeType.Delay);
            upgrade.UpdateValue();
            UpdateCosts();
        }
    }

    public void BuyPoison(Upgrade upgrade) {
        if (CoinManager.Coins >= _currentCost) {
            GameManager.Instance.CoinManager.ReduceCoin(_currentCost);
            BuyUpgrade(UpgradeType.Poison);
            upgrade.UpdateValue();
            UpdateCosts();
        }
    }

    private void BuyUpgrade(UpgradeType upgradeType) {
        print(upgradeType);
        switch (upgradeType) {
            case UpgradeType.Damage:
                _currentWeapon.ImproveDamage();
                break;
            case UpgradeType.Delay:
                _currentWeapon.ImproveDelay();
                break;
            case UpgradeType.Poison:
                _currentWeapon.ImprovePoison();
                break;
        }
        SetWeaponToUpgrades();
    }

    private void SetWeaponToUpgrades() {
        foreach (var upgrade in _upgrades) {
            upgrade.CurrentWeapon = _currentWeapon;
            upgrade.UpdateValue();
        }
    }


    public void GoToBattle() {
        GameManager.Instance.EventManager.ImprovedWeapon(_currentWeapon);
    }

}