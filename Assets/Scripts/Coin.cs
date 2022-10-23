using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceToMove;
    [SerializeField] private float _distanceToPickUp;
    [SerializeField] private int _coinCount = 10;

    Transform _targetPlayer;

    private void Start() {
        _targetPlayer = GameManager.Instance.RecorderController.GetActivePlayer().transform;
        GameManager.Instance.EventManager.OnRoundFinished += OnRoundFinished;
    }

    private void OnRoundFinished() {
        Die();
    }

    private void Update() {
        if (_targetPlayer != null) {
            if(Vector3.Distance(transform.position, _targetPlayer.position) < _distanceToMove)
                transform.position = Vector3.MoveTowards(transform.position, _targetPlayer.position, _speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetPlayer.position) < _distanceToPickUp)
                PickUpCoin();
        }
    }

    private void PickUpCoin() {
        GameManager.Instance.EventManager.PickedUpCoin(_coinCount);
        Die();
    }

    private void Die() {
        GameManager.Instance.EventManager.OnRoundFinished -= OnRoundFinished;
        Destroy(gameObject);
    }
}
