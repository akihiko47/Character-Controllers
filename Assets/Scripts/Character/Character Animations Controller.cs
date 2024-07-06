using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationsController : MonoBehaviour {

    [SerializeField]
    private float turnTime = 0.2f;

    [SerializeField]
    private CharacterMovement movementScript;

    private Animator animator;

    private float rotationVelocity;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        Vector2 input = movementScript.GetPlayerInput();

        RotateCharacterToInput(input);

        if (input != Vector2.zero) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            animator.SetBool("isRunning", true);
        } else {
            animator.SetBool("isRunning", false);
        }
    }

    public void RotateCharacterToInput(Vector2 dir) {
        if (dir == Vector2.zero) {
            return;
        }

        float targetRotation = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, turnTime);
    }

}
