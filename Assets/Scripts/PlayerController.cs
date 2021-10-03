using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Settings
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float movementSmoothing = 20f;
    [SerializeField] private float jumpHeight = 1.0f;

    // Dependencies
    private Transform _camTransform;
    private Camera _cam;
    private CharacterController _controller;
    
    // Globals
    private float _cameraPitch;
    private float _yVelocity = 0f;
    private Vector2 _smoothInput = Vector2.zero;
    private Vector2 _currentInputVelocity = Vector2.zero;

    private void Start()
    {
        // Get dependencies
        _camTransform = transform.Find("Camera");
        _cam = _camTransform.GetComponent<Camera>();
        _controller = GetComponent<CharacterController>();
        
        // Setup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() 
    {
        UpdateMouseLook();
        UpdateMovement();
        CheckShoot();
    }

    /// <summary>Get mouse input and move camera accordingly.</summary>
    private void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        _cameraPitch -= mouseDelta.y * mouseSensitivity;
        _cameraPitch = Mathf.Clamp( _cameraPitch, -90f, 90f);
        _camTransform.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    /// <summary>Get movement input and apply it to the player controller.</summary>
    private void UpdateMovement()
    {
        // Get input
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementInput.Normalize();

        _smoothInput = Vector2.SmoothDamp(_smoothInput, movementInput, ref _currentInputVelocity,
            movementSmoothing * Time.deltaTime);
        
        // Apply gravity
        if (_controller.isGrounded)
        {
            _yVelocity = 0f;
        }

        _yVelocity += gravity * Time.deltaTime;
        
        // Apply jump movement
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _yVelocity += jumpHeight;
        }
        
        // Calculate player movement this frame
        Vector3 velocity = (transform.forward * _smoothInput.y + transform.right * _smoothInput.x) * walkSpeed;
        velocity += Vector3.up *_yVelocity;

        _controller.Move(velocity * Time.deltaTime);
    }

    private void CheckShoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 rayOrigin = _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, _camTransform.forward, out hit))
            {
                EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}
