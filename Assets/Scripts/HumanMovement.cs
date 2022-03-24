using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour {

    private Animator animator;
    private CharacterController characterController;
    
    private float horizontalMovement = 0f;
    private float verticalMovement = 0f;
    private bool intendsToRun = false;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    void Awake() {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
   
    void Update() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        intendsToRun = Input.GetKey(KeyCode.LeftShift);
    }

    void FixedUpdate() {
        Vector3 direction = new Vector3(horizontalMovement, 0, verticalMovement);
        direction.Normalize();

        Vector3 transformDirection = transform.TransformDirection(direction);

        float speed = (intendsToRun) ? runSpeed : walkSpeed;
        Vector3 movement = speed * Time.deltaTime * transformDirection;

        characterController.Move(movement);

        float factor = (intendsToRun) ? 2f : 1f;
        animator.SetFloat("VelocityX", horizontalMovement * factor, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityZ", verticalMovement * factor, 0.1f, Time.deltaTime);
    }
}
