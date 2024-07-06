using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {

    [SerializeField, Min(0.0f)]
    private float walkSpeed = 1.8f;

    [SerializeField, Min(0.0f)]
    private float runSpeed = 5f;

    [SerializeField, Min(0.0f)]
    private float jumpHeight = 1f;

    [SerializeField]
    private float gravityModifier = 3f;

    [SerializeField]
    private Transform inputSpace = default;

    private CharacterController controller;

    private bool running = false;

    private Vector2 input;
    private Vector3 moveDirection;
    private Vector3 velocity;

    private float gravity;

    private void Start() {
        controller = GetComponent<CharacterController>();

        gravity = Physics.gravity.y * gravityModifier;
    }

    private void Update() {
        GetInput();

        Move();
    }

    private void GetInput() {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        running = Input.GetKey(KeyCode.LeftShift);
    }

    private void Move() {
        moveDirection = new Vector3(input.x, 0.0f, input.y);

        if (inputSpace) {
            Vector3 forward = inputSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = inputSpace.right;
            right.y = 0f;
            right.Normalize();
            moveDirection = (forward * input.y + right * input.x).normalized;
        }

        float targetSpeed = (running ? runSpeed : walkSpeed);
        velocity = moveDirection * targetSpeed;

        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
    }

    /*private void Jump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }*/

    private void ApplyGravity() {
        velocity.y += gravity;

        if (controller.isGrounded) {
            velocity.y = 0f;
        }
    }

    public Vector3 GetVelocity() {
        return velocity;
    }

    public float GetVelocityPercent() {
        float currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        return (running ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
    }
}
