using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    [SerializeField]
    private float sensitivity = 10f;

    [SerializeField]
    private float rotationSmoothTime = 0.12f;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float orbitRadius = 2f;

    [SerializeField]
    private Vector2 pitchMinMax = new Vector2(-40f, 85f);

    private Vector3 _currentRotation, _rotationVelocity;

    float _yaw, _pitch;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() {
        GetRotation();
        ClampRotation();
        Rotate();
        Move();
    }

    private void GetRotation() {
        _yaw += Input.GetAxis("Mouse X") * sensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * sensitivity;
    }

    private void ClampRotation() {
        _pitch = Mathf.Clamp(_pitch, pitchMinMax.x, pitchMinMax.y);
    }

    private void Rotate() {
        _currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_pitch, _yaw), ref _rotationVelocity, rotationSmoothTime);
        transform.eulerAngles = _currentRotation;
    }

    private void Move() {
        transform.position = target.position - transform.forward * orbitRadius;

        if (Physics.Raycast(target.position, -transform.forward, out RaycastHit hit, orbitRadius)) {
            transform.position = target.position - transform.forward * (hit.distance - 0.1f);
        }
    }

    public void SetSensitivity(float newSens) {
        sensitivity = newSens;
    }
}
