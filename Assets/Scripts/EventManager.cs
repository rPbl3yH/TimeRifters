using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Action OnGameStarted;
    public Action<int> OnNextRoundStarted;
    public Action OnRoundFinished;
    public Action<Weapon> OnShooted;
    public Action<Weapon> OnChoseWeapon;
    public Action<Weapon> OnImprovedWeapon;
    public Action<int> OnPickedUpCoin;
    public Action OnWinGame;

    public void NextRoundStarted(int value) => OnNextRoundStarted?.Invoke(value);

    public void GameStarted() => OnGameStarted?.Invoke();

    public void RoundFinished() => OnRoundFinished?.Invoke();

    public void Shooted(Weapon weapon) => OnShooted?.Invoke(weapon);

    public void ImprovedWeapon(Weapon weapon) => OnImprovedWeapon?.Invoke(weapon);

    public void WinGame() => OnWinGame?.Invoke();

    public void ChoseWeapon(Weapon weapon) {
        print("We chose " + weapon.name);
        OnChoseWeapon?.Invoke(weapon);
    }

    public void PickedUpCoin(int value) {
        OnPickedUpCoin?.Invoke(value);
    }
}
