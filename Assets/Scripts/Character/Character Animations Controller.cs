using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationsController : MonoBehaviour {

    [SerializeField]
    private float turnTime = 0.2f;

    [SerializeField]
    private float animationChangeTime = 0.1f;

    [SerializeField]
    private CharacterMovement movementScript;

    private Animator animator;

    private float rotationVelocity;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        Vector3 velocity = movementScript.GetVelocity();

        animator.SetFloat("Velocity", movementScript.GetVelocityPercent(), animationChangeTime, Time.deltaTime);

        RotateCharacter(velocity);
    }

    public void RotateCharacter(Vector3 dir) {
        if (new Vector2(dir.x, dir.z) == Vector2.zero) {
            return;
        }

        float targetRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, turnTime);
    }

}
