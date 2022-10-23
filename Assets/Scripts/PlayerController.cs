using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string AnimIsWalk = "IsWalk";
    public Animator Animator;

    public bool IsPlaying;

    [SerializeField] private Transform _cameraCenter;
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _rotateSpeedY;
    [SerializeField] private float _rotateSpeedX;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [SerializeField] private Weapon[] _weapons;
    [SerializeField] public Weapon CurrentWeapon { private set; get; }

    [SerializeField] private float _minRotateX, _maxRotateX;
    [SerializeField] private Aim _aim;

    private bool _isGrounded;

    private float _rotationX;
    private void Start() {
        //Cursor.visible = false;
        GameManager.Instance.EventManager.OnImprovedWeapon += OnImprovedWeapon;
    }

    

    private void Update() {

        if (IsPlaying) {
            if (_camera.gameObject.activeSelf) _camera.gameObject.SetActive(false);
            if (!_rigidbody.isKinematic) _rigidbody.isKinematic = true;
            return;
        }

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(xAxis, 0, yAxis);

        if (direction.magnitude > 0) {
            if (_rigidbody.velocity.magnitude < _maxSpeed) {
                _rigidbody.AddRelativeForce(direction * Time.deltaTime * _walkSpeed, ForceMode.VelocityChange);

            }
            _animator.SetBool(AnimIsWalk, true);
        }
        else {
            _rigidbody.velocity = Vector3.zero;
            _animator.SetBool(AnimIsWalk, false);
        }

        Rotate();

        Jump(); 
    }

    private void Jump() {
        if (!_isGrounded) return;
        if (Input.GetKeyDown(KeyCode.Space)) {
            _rigidbody.AddRelativeForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (Vector3.Angle(collision.contacts[0].normal, Vector3.up) < 30f) {
            _isGrounded = true;
        }
        else
            _isGrounded = false;
    }

    private void Rotate() {
        float horMouseAxis = Input.GetAxis("Mouse X");
        if (horMouseAxis != 0) {
            Vector3 rotateXDireciton = new Vector3(0, horMouseAxis, 0);
            _rigidbody.angularVelocity = rotateXDireciton * _rotateSpeedY;
        }
        else {
            _rigidbody.angularVelocity = Vector3.zero;
        }
        float vertMouseAxis = Input.GetAxis("Mouse Y");

        if (vertMouseAxis != 0) {
            _rotationX -= vertMouseAxis * _rotateSpeedX * Time.deltaTime;
            _rotationX = Mathf.Clamp(_rotationX, _minRotateX, _maxRotateX);

            _cameraCenter.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
            //_cameraCenter.Rotate(rotateYDirection);
        }
    }

    public void Shoot() {
        CurrentWeapon.Shoot();
        print("Shoot");
    }

    public void OffRotation() {
        _rigidbody.constraints = _rigidbody.constraints | RigidbodyConstraints.FreezeRotationY;
    }

    public void SetCurrentWeapon(Weapon value) {
        if (CurrentWeapon != null) return;
        foreach (var weapon in _weapons) {
            if (weapon.GetType() == value.GetType()) {
                CurrentWeapon = weapon;
                weapon.gameObject.SetActive(true);
                weapon.Initialize(value);
                weapon.PrintInfo();
                weapon.Aim = _aim;
            }
            else {
                weapon.gameObject.SetActive(false);
            }
        }

    }

    private void OnImprovedWeapon(Weapon weapon) {
        SetCurrentWeapon(weapon);
    }

    public void DeactivateCamera() {
        _camera.gameObject.SetActive(false);
    }
}