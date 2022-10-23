using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int CountEnemis { private set; get; }

    [SerializeField] private Enemy _prefabEnemy;
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private float _delayToSpawn;
    [SerializeField] private int _spawnCount;
    private int _spawnedCount;
    private int _remainingCount;

    [SerializeField] private List<Enemy> _currentEnemies = new List<Enemy>();
    private Way _way;
    private Coroutine _currentCoroutine;

    public void SetWay(Way value) => _way = value;

    private void Start() {
        GameManager.Instance.EventManager.OnNextRoundStarted += OnNextRoundStarted;
        GameManager.Instance.EventManager.OnRoundFinished += OnRoundFinished;
        _remainingCount = _spawnCount;
        UpdateEnemiesText();
    }

    private void OnRoundFinished() {
        ClearAllEnemies();
        _spawnedCount = 0;
        _remainingCount = _spawnCount;
        if (_currentCoroutine != null) {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    private void OnNextRoundStarted(int obj) {
        
        if (_currentCoroutine == null)
            _currentCoroutine = StartCoroutine(StartSpawn());
    }

    private void Update() {
        
    }

    IEnumerator StartSpawn() {
        yield return new WaitForSeconds(_timeToSpawn);
        for (int i = 0; i < _spawnCount; i++) {
            Spawn();
            yield return new WaitForSeconds(_delayToSpawn);
        }
    }

    private void Spawn() {
        _prefabEnemy.SetWay(_way);
        Enemy enemy = Instantiate(_prefabEnemy);
        _currentEnemies.Add(enemy);
        CountEnemis++;
        _spawnedCount++;
        UpdateEnemiesText();
    }

    private void UpdateEnemiesText() {  
        GameManager.Instance.UIController.UpdateEnemiesText(_remainingCount);
    }

    public void SendMessageDie(Enemy enemy) {
        _currentEnemies.Remove(enemy);
        CountEnemis--;
        _remainingCount--;
        UpdateEnemiesText();
        if(_remainingCount <= 0 && _spawnedCount >= _spawnCount) {
            GameManager.Instance.EventManager.WinGame();
        }
    }

    public void ClearAllEnemies() {
        var count = _currentEnemies.Count;
        for (int i = 0; i < count; i++) {
            Destroy(_currentEnemies[0].gameObject);
            _currentEnemies.RemoveAt(0);
        }
    }
}
