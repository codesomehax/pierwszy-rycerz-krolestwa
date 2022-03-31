using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour {

    private Animator _animator;
    private CharacterController _characterController;
    
    private float _horizontalMovement = 0f;
    private float _verticalMovement = 0f;
    private bool _intendsToRun = false;

    public float WalkSpeed = 2f;
    public float RunSpeed = 5f;
    public float FallSpeed = 2f;
    public Transform CameraTransform;

    void Awake() {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }
   
    void Update() {
        _horizontalMovement = Input.GetAxis("Horizontal");
        _verticalMovement = Input.GetAxis("Vertical");
        _intendsToRun = Input.GetKey(KeyCode.LeftShift);
    }

    void FixedUpdate() {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, CameraTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
        if (_horizontalMovement != 0f || _verticalMovement != 0f || !_characterController.isGrounded) {
            Vector3 direction = new Vector3(_horizontalMovement, 0, _verticalMovement);
            direction.Normalize();

            Vector3 transformDirection = transform.TransformDirection(direction);

            float speed = (_intendsToRun) ? RunSpeed : WalkSpeed;
            if (!_characterController.isGrounded) {
                transformDirection.y = -FallSpeed;
            }

            Vector3 movement = speed * Time.deltaTime * transformDirection;

            _characterController.Move(movement);

            float factor = (_intendsToRun) ? 2f : 1f;
            _animator.SetFloat("VelocityX", _horizontalMovement * factor, 0.1f, Time.deltaTime);
            _animator.SetFloat("VelocityZ", _verticalMovement * factor, 0.1f, Time.deltaTime);
        }
    }

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
