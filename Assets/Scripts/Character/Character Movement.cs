using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {

    [Header("Input")]
    [SerializeField]
    private Transform inputSpace = default;

    [Header("Walking")]
    [SerializeField, Min(0.0f)]
    private float walkSpeed = 1.8f;

    [Header("Running")]
    [SerializeField]
    private bool canRun = true;

    [SerializeField, Min(0.0f)]
    private float runSpeed = 5f;

    [Header("Jumping")]
    [SerializeField]
    private bool canJump = true;

    [SerializeField, Min(0.0f)]
    private float jumpHeight = 1f;

    [SerializeField, Min(0.01f)]
    private float onGroundThreshold = 0.1f;

    [SerializeField]
    private float gravityModifier = 3f;

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
        AdjustContactNormal();
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

        if (inputSpace) {
            Vector3 forward = inputSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = inputSpace.right;
            right.y = 0f;
            right.Normalize();
            _moveDirection = (forward * _input.y + right * _input.x).normalized;
        }

        Debug.Log(_moveDirection);
        _moveDirection = Vector3.ProjectOnPlane(_moveDirection, _contactNormal).normalized;
        Debug.Log(_moveDirection);
        Debug.Log(" ");

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

    private void AdjustContactNormal() {
        if (_timeSinceLastGrounded < onGroundThreshold) {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.2f)) {
                _contactNormal = hit.normal;
            }
        } else {
            _contactNormal = Vector3.up;
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
