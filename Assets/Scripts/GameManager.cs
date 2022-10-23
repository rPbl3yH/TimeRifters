using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public EventManager EventManager { get; private set; }
    [field: SerializeField] public UIController UIController { get; private set; }
    [field: SerializeField] public CoinManager CoinManager { get; private set; }
    [field: SerializeField] public RecorederController RecorderController { get; private set; }
    [field: SerializeField] public EnemySpawner EnemySpawner { get; private set; }
    [field: SerializeField] public EnemyWayCotroller EnemyWayCotroller { get; private set; }
    [field: SerializeField] public WeaponSettings WeaponSettings { get; private set; }

    [SerializeField] private float _durationGame = 15f;
    public float TimerRound { get; set; }

    private int _currentIdRound;
    public bool IsGame { get; set; }

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else {
            //Destroy(gameObject);
        }
    }

    private void Start() {
        IsGame = true;
        EventManager.OnImprovedWeapon += OnImprovedWeapon;
        EventManager.OnWinGame += OnWinGame;
    }

    private void OnWinGame() {
        IsGame = false;
        Cursor.visible = true;
        CancelInvoke();
    }

    private void Update() {
        if(TimerRound > 0 && IsGame)
            TimerRound -= Time.deltaTime;
        UIController.SetTimerText();

    }

    private void OnImprovedWeapon(Weapon obj) => StartNextRound();

    private void OnRoundFinished() => _currentIdRound++;

    public int GetActiveIdRound() => _currentIdRound;

    public void StartNextRound() {
        Cursor.visible = false;
        EventManager.NextRoundStarted(_currentIdRound);
        Invoke(nameof(EndRound), _durationGame);
        ResetTimer();
    }

    private void EndRound() {
        print("Round " + _currentIdRound + " Over");
        _currentIdRound++;
        Cursor.visible = true;
        
        if (EnemySpawner.CountEnemis > 0) {
            EventManager.RoundFinished();
        }
        
    }

    public void StartGame() => EventManager.GameStarted();

    private void ResetTimer() {
        TimerRound = _durationGame;
    }

    public void RestartGame() {
        RecorderController.Delete();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnApplicationQuit() {
        RecorderController.Delete();
    }
}
