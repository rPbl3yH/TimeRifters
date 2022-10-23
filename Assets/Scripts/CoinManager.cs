using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static int Coins { private set; get; }

    [SerializeField] private TMP_Text _coinsGameText;
    [SerializeField] private int _startCountCoin = 100;

    private void Start() {
        GameManager.Instance.EventManager.OnPickedUpCoin += OnPickedUpCoin;
        GameManager.Instance.EventManager.OnNextRoundStarted += OnNextRoundStarted;
        GameManager.Instance.EventManager.OnRoundFinished += OnRoundFinished;

        _coinsGameText.enabled = false;
        Coins = _startCountCoin;
        UpdateText();
    }

    private void OnRoundFinished() {
        _coinsGameText.enabled = false;
    }

    public void DeactivateCoinText() {
        _coinsGameText.enabled = false;
    }

    private void OnNextRoundStarted(int obj) {
        _coinsGameText.enabled = true;
    }

    private void OnPickedUpCoin(int value) {
        Coins+= value;
        UpdateText();
    }

    private void UpdateText() {
        _coinsGameText.text = "Coins " + Coins;
    }

    public void ReduceCoin(int value) {
        Coins -= value;
        Coins = Mathf.Clamp(Coins, 0, int.MaxValue);
        UpdateText();
    }
}
