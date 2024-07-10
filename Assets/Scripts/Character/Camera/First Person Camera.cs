using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {

    [SerializeField]
    private float sensitivity = 10f;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform eyes;

    private float _xRotation = 0f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        playerTransform.Rotate(Vector3.up * mouseX);

        transform.rotation = Quaternion.Euler(_xRotation, playerTransform.eulerAngles.y, 0f);
        transform.position = eyes.position;
    }

    public void SetSensitivity(float newSens) {
        sensitivity = newSens;
    }
}
