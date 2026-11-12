using UnityEngine;

namespace Nexus.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        [Header("Movimiento")]
        [SerializeField] private float _walkSpeed = 6.0f;
        [SerializeField] private float _sprintSpeed = 9.5f;
        [SerializeField] private float _crouchSpeed = 3.5f;
        [SerializeField] private float _jumpHeight = 1.6f;
        [SerializeField] private float _gravity = -20f;

        [Header("Cámara")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _mouseSensitivity = 2.0f;
        [SerializeField] private float _pitchLimit = 85.0f;

        private CharacterController _controller;
        private Vector3 _velocity;
        private float _cameraPitch = 0f;
        private bool _isGrounded;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleLook();
            HandleMovement();
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * _mouseSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;

            _cameraPitch -= mouseY;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -_pitchLimit, _pitchLimit);

            if (_playerCamera != null)
            {
                _playerCamera.transform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
            }

            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            _isGrounded = _controller.isGrounded;
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            bool isCrouching = Input.GetKey(KeyCode.C);

            float speed = isCrouching ? _crouchSpeed : (isSprinting ? _sprintSpeed : _walkSpeed);

            _controller.Move(moveDirection * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}
