using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private TMP_Text _textDamage;
    [SerializeField] private TMP_Text _textTitle;

    [SerializeField] private float _timeToScale = 2f;

    private float _time;
    private float _startWeaponScale;

    private Coroutine _coroutineScale;
    private Vector3 _targetScale;
    private void Awake() {
        
    }

    private void Start() {
        InitializeAllWeapons();
        foreach (var weapon in _weapons) {
            weapon.gameObject.SetActive(false);
        }

        foreach(var weaponSetting in GameManager.Instance.WeaponSettings.WeaponsSettings) {
            if(_currentWeapon.WeaponType == weaponSetting.WeaponType) {
                _currentWeapon.gameObject.SetActive(true);
                _textDamage.text = $"DMG: {_currentWeapon.Damage}";
                _textTitle.text = $"{_currentWeapon.name}";
            }
        }

        _startWeaponScale = _currentWeapon.transform.localScale.x;
    }

    private void Update() {
        _time += Time.deltaTime;
        var sin01 = (Mathf.Sin(_time) + 2f)/2f;

        if(sin01 > 0) {
            
        }

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

    public void ChooseWeapon() {
        GameManager.Instance.EventManager.ChoseWeapon(_currentWeapon);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _targetScale = new Vector3(_startWeaponScale, _startWeaponScale, _startWeaponScale) * 1f;
        SetTargetScale();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _targetScale = new Vector3(_startWeaponScale, _startWeaponScale, _startWeaponScale) * 1.3f;
        SetTargetScale();
    }

    private void SetTargetScale() {
        if (_coroutineScale != null) {
            StopCoroutine(_coroutineScale);
        }
        _coroutineScale = StartCoroutine(SetScale());
    }


    IEnumerator SetScale() {
        for (float t = 0; t < 1f; t+= Time.deltaTime / _timeToScale) {
            _currentWeapon.transform.localScale = Vector3.Lerp(_currentWeapon.transform.localScale, _targetScale, t);
            yield return null;
        }
    }
}
