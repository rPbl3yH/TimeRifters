using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Way
{
    public Vector3[] Points;
}

public class EnemyWayCotroller : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;

    [SerializeField] private Way _way;

    [SerializeField] private float _sizeX;
    [SerializeField] private float _sizeY;
    [SerializeField] private float _sizeZ;

    private int _currentWave;
    private void Awake() {
        _way.Points = new Vector3[_wayPoints.Length];
    }

    private void Start() {
        
        SetRandomPositionToPoints();
        GameManager.Instance.EnemySpawner.SetWay(_way);
    }

    private void SetRandomPositionToPoints() {

        if (_wayPoints.Length == 0) return;
        SetUpStartAndEndWayPoint();

        for (int i = 1; i < _wayPoints.Length - 1; i++) {
            SetCurrentWayPoint(i, GetRandomPosition());
            //SetCurrentWayPoint(i, _wayPoints[i].position);
        }
    }

    private void SetUpStartAndEndWayPoint() {
        SetCurrentWayPoint(0, _startPoint.position);
        SetCurrentWayPoint(_wayPoints.Length - 1, _endPoint.position);
    }

    private void SetCurrentWayPoint(int id, Vector3 point) {
        _way.Points[id] = point;
        _wayPoints[id].position = point;
    }

    private void OnDrawGizmos() {
        var size = new Vector3(_sizeX, _sizeY, _sizeZ);
        Gizmos.DrawWireCube(transform.position + new Vector3(0f,_sizeY/2,0f), size);
    }

    private Vector3 GetRandomPosition() {
        float posX = Random.Range(-_sizeX / 2, _sizeX / 2);
        float posY = Random.Range(0, _sizeY);
        float posZ = Random.Range(-_sizeZ / 2, _sizeZ / 2);
        return new Vector3(posX, posY, posZ);   
    }
}
