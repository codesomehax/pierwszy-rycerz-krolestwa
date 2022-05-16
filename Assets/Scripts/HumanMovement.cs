using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour {

    private Animator _animator;
    private CharacterController _characterController;
    private Player _player;
    
    private float _horizontalMovement = 0f;
    private float _verticalMovement = 0f;
    private bool _intendsToRun = false;
    private float _walkSpeed;
    private float _runSpeed;
    private float _fallSpeed;

    public Transform CameraTransform;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<Player>();
        _walkSpeed = _player.WalkSpeed;
        _runSpeed = _player.RunSpeed;
        _fallSpeed = _player.FallSpeed;
    }
   
    void Update()
    {
        _horizontalMovement = Input.GetAxis("Horizontal");
        _verticalMovement = Input.GetAxis("Vertical");
        _intendsToRun = Input.GetKey(KeyCode.LeftShift);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, CameraTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime * 10f); 
        
        if (_horizontalMovement != 0f || _verticalMovement != 0f || !_characterController.isGrounded)
        {
            Vector3 direction = new Vector3(_horizontalMovement, 0, _verticalMovement);
            direction.Normalize();

            Vector3 transformDirection = transform.TransformDirection(direction);

            float speed = (_intendsToRun) ? _runSpeed : _walkSpeed;
            if (!_characterController.isGrounded)
            {
                transformDirection.y = -_fallSpeed;
            }

            Vector3 movement = speed * Time.deltaTime * transformDirection;

            _characterController.Move(movement);

            float factor = (_intendsToRun) ? 2f : 1f;
            _animator.SetFloat("VelocityX", _horizontalMovement * factor, 0.1f, Time.deltaTime);
            _animator.SetFloat("VelocityZ", _verticalMovement * factor, 0.1f, Time.deltaTime);
        }

        if (
            (_animator.GetFloat("VelocityX") > 0f && _animator.GetFloat("VelocityX") < 0.05f) ||
            (_animator.GetFloat("VelocityX") < 0f && _animator.GetFloat("VelocityX") > -0.05f)
        )
        {
            _animator.SetFloat("VelocityX", 0f);
        }

        if (
            (_animator.GetFloat("VelocityZ") > 0f && _animator.GetFloat("VelocityZ") < 0.05f) ||
            (_animator.GetFloat("VelocityZ") < 0f && _animator.GetFloat("VelocityZ") > -0.05f)
        )
        {
            _animator.SetFloat("VelocityZ", 0f);
        }
    }

    void OnDisable()
    {
        _animator.SetFloat("VelocityX", 0f);
        _animator.SetFloat("VelocityZ", 0f);
    }
}
