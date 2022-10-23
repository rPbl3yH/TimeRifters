using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private Recorder _recorder;

    private void Awake() {
        _recorder.AddStartChild(transform);
    }
}
