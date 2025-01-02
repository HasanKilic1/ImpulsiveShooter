using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnJump;

    public static PlayerController Instance;
    
    [SerializeField] private Transform feetTransform;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpStrength = 7f;
    [SerializeField] private float extraGravity = 700f;
    [SerializeField] private float gravityDelay = 0.2f;
    [SerializeField] private float coyoteTime = 0.5f;

    private float timeInAir;
    private float coyoteTimer;
    private bool doubleJumpAvailable;

    private PlayerInput playerInput;
    private FrameInput frameInput;
    private Movement movement;
    private Vector2 _movement;
    private Rigidbody2D _rigidBody;

    public void Awake()
    {
        if (Instance == null) { Instance = this; }

        playerInput = GetComponent<PlayerInput>();
        _rigidBody = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
    }

    private void Update()
    {
        GatherInput();
        Movement();
        HandleJump();
        GravityDelay();
        HandleSpriteFlip();
        CoyoteTimer();
    }

    private void FixedUpdate()
    {
        ApplyExtraGravity();
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GatherInput()
    {
        frameInput = playerInput.FrameInput;
    }

    private void Movement()
    {
        movement.SetCurrentXDirection(frameInput.Move.x);
    }

    private void HandleJump()
    {
        if (!frameInput.Jump)
            return;

        if (CheckGrounded())
        {
            OnJump?.Invoke();
        }
        else if(coyoteTimer > 0)
        {
            OnJump?.Invoke();
        }
        else if (doubleJumpAvailable)
        {
            doubleJumpAvailable = false;
            OnJump?.Invoke();
        }
    }

    private void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            coyoteTimer = coyoteTime;
            doubleJumpAvailable = true;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void ApplyJumpForce()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        timeInAir = 0f;
        coyoteTimer = 0f;
        _rigidBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }

    private bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(feetTransform.position, groundCheck, 0f, groundLayer);
        return isGrounded != null;
    }

    private void GravityDelay()
    {
        if (!CheckGrounded())
        {
            timeInAir += Time.deltaTime;
        }
        else
        {
            timeInAir = 0f;
        }
    }

    private void ApplyExtraGravity()
    {
        if(timeInAir > gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f , -extraGravity * Time.deltaTime));
        }
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetTransform.position, groundCheck);
    }
}
