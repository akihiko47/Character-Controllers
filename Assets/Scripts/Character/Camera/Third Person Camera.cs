using System.Collections;
using System.Collections.Generic;
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

    private Vector3 currentRotation, rotationVelocity;

    float yaw, pitch;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() {
        GetRotation();
        ClampRotation();
        Rotate();
    }

    private void GetRotation() {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
    }

    private void ClampRotation() {
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
    }

    private void Rotate() {
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * orbitRadius;
    }
}
