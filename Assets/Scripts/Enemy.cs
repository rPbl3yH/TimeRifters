using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IPoisonable
{
    void StartPoisonDamage(int damage);
}

public class Enemy : MonoBehaviour, IDamageable, IPoisonable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Transform[] _quadsHalth;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _goldPrefab;
    [SerializeField] private int _minGold, _maxGold;
    [SerializeField] private float _delayPoisonDamage;
    [SerializeField] private float _durationPoisonEffect = 5f;
    [SerializeField] private GameObject _poisonEffect;
    [SerializeField] private GameObject _visual;
    [SerializeField] private Collider _collider;

    [SerializeField] private Way _currentWay;

    private Vector3 _targetPoint;
    private int _currentHeatlh, _currentIdWayPoint;
    private bool _isPoisonDamage;
    private Coroutine _currentCoroutine;

    private void Start() {
        _currentHeatlh = _maxHealth;
        transform.position = _currentWay.Points[0];
        SetNextWayPoint();
    }

    private void Update() {
        
        if(Vector3.Distance(_targetPoint, transform.position) < 0.01f) {
            SetNextWayPoint();
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetPoint, _speed * Time.deltaTime);
    }

    private void SetNextWayPoint() {
        if (_currentIdWayPoint + 1 >= _currentWay.Points.Length) {
            _visual.SetActive(false);
            _collider.enabled = false;
            return;
        }
            
        _currentIdWayPoint++;
        _targetPoint = _currentWay.Points[_currentIdWayPoint];
    }

    public void TakeDamage(int damage) {
        _currentHeatlh -= damage;
        if(_currentHeatlh <= 0) {
            Die();
        }
        SetVisualHealth();
        print("Current health " + _currentHeatlh); 
    }

    private void Die() {
        GameManager.Instance.EnemySpawner.SendMessageDie(this);
        CreateGolds();

        Destroy(gameObject);
    }

    [ContextMenu("Create goldes")]
    private void CreateGolds() {
        var countGold = Random.Range(_minGold, _maxGold);
        for (int i = 0; i < countGold; i++) {
            var randomPos = new Vector3(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            var spawnPos = transform.position + randomPos;
            Instantiate(_goldPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void SetVisualHealth() {
        
        float newScale = _currentHeatlh / (float)_maxHealth;
        newScale = Mathf.Clamp01(newScale);
        foreach (var quad in _quadsHalth) {
            quad.localScale = new Vector3(quad.localScale.x, newScale, quad.localScale.z);
        }
    }

    public void SetWay(Way value) => _currentWay = value;

    public void StartPoisonDamage(int damage) {
        _isPoisonDamage = true;
        if(!_poisonEffect.activeSelf)
            _poisonEffect.SetActive(true);
        if (_currentCoroutine == null) {
            _currentCoroutine = StartCoroutine(TakePoisonDamage(damage));
        }
    }

    private IEnumerator TakePoisonDamage(int damage) {
        while (_isPoisonDamage) {
            TakeDamage(damage / 3);
            yield return new WaitForSeconds(_delayPoisonDamage);
        }
        _poisonEffect.SetActive(false);
        _currentCoroutine = null;
    }
}
