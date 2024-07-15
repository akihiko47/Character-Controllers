using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {

    [SerializeField, Min(0.0f)]
    private float walkSpeed = 1.8f;

    [Space(15)]
    [SerializeField]
    private bool canRun = true;

    [SerializeField, Min(0.0f)]
    private float runSpeed = 5f;

    [Space(15)]
    [SerializeField]
    private bool canJump = true;

    [SerializeField, Min(0.0f)]
    private float jumpHeight = 1f;

    [Space(15)]
    [SerializeField, Min(0.01f)]
    private float onGroundThreshold = 0.1f;

    [SerializeField]
    private float gravityModifier = 3f;

    [SerializeField]
    private Transform inputSpace = default;

    private CharacterController _controller;

    private bool _running = false;
    private bool _jumping = false;

    private Vector2 _input;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private Vector3 _contactNormal;
    private float _velocityY = 0f;

    private float _gravity;

    private float _timeSinceLastGrounded;

    private void Start() {
        _controller = GetComponent<CharacterController>();

        _gravity = Physics.gravity.y * gravityModifier;
    }

    private void Update() {
        GetInput();
        Move();

        ConfigureTimeSinceGrounded();
        ConfigureJumpState();
    }

    private void GetInput() {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        _running = canRun && Input.GetKey(KeyCode.LeftShift);

        if (canJump && Input.GetKey(KeyCode.Space) && _timeSinceLastGrounded < onGroundThreshold && !_jumping) {
            Jump();
        }
    }

    private void Move() {
        _moveDirection = new Vector3(_input.x, 0.0f, _input.y);
        _moveDirection = Vector3.ProjectOnPlane(_moveDirection, _contactNormal);

        if (inputSpace) {
            Vector3 forward = inputSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = inputSpace.right;
            right.y = 0f;
            right.Normalize();
            _moveDirection = (forward * _input.y + right * _input.x).normalized;
        }

        float targetSpeed = (_running ? runSpeed : walkSpeed);

        ApplyGravity();

        _velocity = _moveDirection * targetSpeed + Vector3.up * _velocityY;
        
        _controller.Move(_velocity * Time.deltaTime);

        if (_controller.isGrounded) {
            _velocityY = -2f;
        }
    }

    private void Jump() {
        _velocityY = Mathf.Sqrt(jumpHeight * -2f * _gravity);
        _jumping = true;
        _contactNormal = Vector3.up;
    }

    private void ApplyGravity() {
        if (!_controller.isGrounded) {
            _velocityY += _gravity * Time.deltaTime;
        }
    }

    private void ConfigureJumpState() {
        // stop jump at ceiling
        if ((_controller.collisionFlags & CollisionFlags.Above) != 0 && !_controller.isGrounded && _velocityY > 0f) {
            _velocityY = 0f;
            _jumping = false;
        }

        // stop jumping if going down
        if (_velocityY <= 0f && _jumping) {
            _jumping = false;
        }
    }

    private void ConfigureTimeSinceGrounded() {
        if (!_controller.isGrounded) {
            _timeSinceLastGrounded += Time.deltaTime;
        }

        if (_controller.isGrounded && _timeSinceLastGrounded > 0f) {
            _timeSinceLastGrounded = 0f;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (_timeSinceLastGrounded < onGroundThreshold) {
            _contactNormal = hit.normal;
        }
    }

    public Vector3 GetVelocity() {
        return _velocity;
    }

    public float GetVelocityPercent() {
        float currentSpeed = new Vector2(_controller.velocity.x, _controller.velocity.z).magnitude;

        if (canRun) {
            return (_running ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
        } else {
            return currentSpeed / walkSpeed;
        }
        
    }

    public bool GetOnGround() {
        return _timeSinceLastGrounded < onGroundThreshold;
    }

    public bool GetJumping() {
        return _jumping;
    }

    private void OnDrawGizmos() {
        if (_controller == null) {
            return;
        }

        Gizmos.color = Color.blue;
        Vector3 from = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Gizmos.DrawRay(from, Vector3.ProjectOnPlane(_moveDirection, _contactNormal));

        if (_timeSinceLastGrounded < onGroundThreshold) {
            Gizmos.color = Color.green;
        } else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
