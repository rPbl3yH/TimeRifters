using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RecorederController : MonoBehaviour
{
    [SerializeField] private PlayerController[] _players;
    [SerializeField] private Recorder[] _recorders  = new Recorder[4];
    [SerializeField] private List<PlayerFrames> _recordings = new List<PlayerFrames>();

    [field: SerializeField] public int CurrentPlayerIdRecording { get; private set; }

    private void Awake() {
        for (int i = 0; i < _players.Length; i++) {
            _recorders[i] = _players[i].GetComponentInChildren<Recorder>();
            if (_players[i].gameObject.activeSelf) _players[i].enabled = false;
        }
    }

    private void Start() {
        GameManager.Instance.EventManager.OnNextRoundStarted += OnNextRoundStarted;
        GameManager.Instance.EventManager.OnRoundFinished += OnRoundFinished;
        GameManager.Instance.EventManager.OnImprovedWeapon += OnImprovedWeapon;
        GameManager.Instance.EventManager.OnWinGame += OnWinGame;
    }

    private void OnWinGame() {
        OnRoundFinished();
    }

    private void OnImprovedWeapon(Weapon weapon) {
        _players[CurrentPlayerIdRecording].gameObject.SetActive(true);
        _players[CurrentPlayerIdRecording].SetCurrentWeapon(weapon);
    }

    private void OnRoundFinished() {
        StopCurrentRecording();
        Save();
    }

    private void OnNextRoundStarted(int obj) {
        Load();
        StartPlayAllRecordings();
        ActivateNextPlayer();
        StartRecord();
    }

    private void StartPlayAllRecordings() {
        for (int idRecorder = 0; idRecorder < CurrentPlayerIdRecording; idRecorder++) {
            _recorders[idRecorder].Play();
        }
    }

    private void OnGameStarted() {
    }

    private void StartRecord() {
        _recorders[CurrentPlayerIdRecording].StartRecord();
    }

    private void StopCurrentRecording() {
        _recorders[CurrentPlayerIdRecording].StopRecording();
    }

    private void ActivateNextPlayer() {
        _players[CurrentPlayerIdRecording].gameObject.SetActive(true);
        _players[CurrentPlayerIdRecording].enabled = true;
    }

    public PlayerController GetActivePlayer() => _players[CurrentPlayerIdRecording];


    [ContextMenu("Save recording")]
    public void Save() {
        _recordings.Add(_recorders[CurrentPlayerIdRecording].GetRecording());
        PlayerFrames[] recordings = _recordings.ToArray();
        SaveSystem.Save(recordings);
        CurrentPlayerIdRecording++;
    }

    [ContextMenu("Load recording")]
    public void Load() {
        var recroding = SaveSystem.Load();
        if (recroding == null) return;
        _recordings = recroding.ToList();
        print(_recordings.Count);
        for (int i = 0; i < _recordings.Count; i++) {
            Frame[] frames = _recordings[i].Frames;
            print(frames.Length);
            _recorders[i].SetRecording(frames);
        }
    }

    [ContextMenu("Delete last recording")]
    public void Delete() {
        SaveSystem.DeleteFile();
    }
}
