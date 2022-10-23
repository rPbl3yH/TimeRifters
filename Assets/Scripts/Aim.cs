using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{
    public Color Color;
    [SerializeField] private Image _image;
    [SerializeField] private LineRenderer _lineRenderer;

    private void Start() {
        _image.color = Color;
    }

    public void SetActive(bool value) {
        gameObject.SetActive(value);
    }

    public void SetAimPosition(RaycastHit hit, Vector3 startPos) {
        gameObject.transform.position = hit.point;
        gameObject.transform.rotation = Quaternion.LookRotation(hit.normal);
        _lineRenderer.SetPositions(new Vector3[] {startPos, hit.point });
    }
}
