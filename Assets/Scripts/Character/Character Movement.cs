using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour {

    [SerializeField, Min(0.0f)]
    private float walkSpeed = 5f;

    private CharacterController controller;

    private Vector2 input;

    private void Start() {
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        GetInput();

        Move();
    }

    private void GetInput() {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;;

        /*Vector3 move = (transform.right * x + transform.forward * z);
        if (move.magnitude > 1) {
            move = move.normalized;
        }*/

        
    }

    private void Move() {
        Vector3 move = new Vector3(input.x, 0.0f, input.y);
        controller.Move(move * walkSpeed * Time.deltaTime);
    }

    public Vector2 GetPlayerInput() {
        return input;
    }
}
