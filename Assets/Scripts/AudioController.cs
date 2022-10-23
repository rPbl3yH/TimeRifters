using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    private void Start() {
        GameManager.Instance.EventManager.OnShooted += OnShooted;
    }

    private void OnShooted(Weapon weapon) {
        _audio.clip = weapon.ShootSound;
        _audio.Play();
    }
}
