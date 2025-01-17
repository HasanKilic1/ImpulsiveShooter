using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 MoveInput => frameInput.Move;
    public static Action OnJump;
    public static Action OnJetpack;

    public static PlayerController Instance;

    [SerializeField] private TrailRenderer jetpackTrailRenderer;
    [SerializeField] private Transform feetTransform;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpStrength = 7f;
    [SerializeField] private float extraGravity = 700f;
    [SerializeField] private float gravityDelay = 0.2f;
    [SerializeField] private float coyoteTime = 0.5f;
    [SerializeField] private float jetPackTime = 0.6f;
    [SerializeField] private float jetPackStrength = 10f;
    
    [Header("Grenade")]
    [SerializeField] private Grenade grenadePrefab;
    [SerializeField] private float grenadeLaunchForceMin, grenadeLaunchForceMax;
    [SerializeField] private float grenadeTorque = 15f;
    [SerializeField] private float grenadeCooldown = 3f;

    private float timeInAir;
    private float coyoteTimer;
    private bool doubleJumpAvailable;
    private float grenadeTimer;

    private Coroutine jetPackCoroutine;
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
        OnJetpack += StartJetpack;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
        OnJetpack -= StartJetpack;
    }

    private void Update()
    {
        GatherInput();
        Movement();
        HandleJump();
        GravityDelay();
        HandleSpriteFlip();
        CoyoteTimer();
        JetPack();
        Grenade();
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
        else if (coyoteTimer > 0)
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

    public bool CheckGrounded()
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
        if (timeInAir > gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f, -extraGravity * Time.deltaTime));
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

    private void JetPack()
    {
        if (!frameInput.JetPack || jetPackCoroutine != null) return;

        OnJetpack?.Invoke();
    }

    private void StartJetpack()
    {
        jetpackTrailRenderer.emitting = true;
        jetPackCoroutine = StartCoroutine(JetpackRoutine());
    }

    private IEnumerator JetpackRoutine()
    {
        float time = 0f;
        while (time < jetPackTime)
        {
            time += Time.deltaTime;
            _rigidBody.linearVelocity = Vector2.up * jetPackStrength;

            yield return null;
        }

        jetPackCoroutine = null;
        jetpackTrailRenderer.emitting = false;
    }

    private void Grenade()
    {
        grenadeTimer -= Time.deltaTime;
        if(frameInput.Grenade && grenadeTimer < 0f)
        {
            Grenade newGrenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            Vector2 launchDir = (Vector2)(Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f , 20f)) * transform.right);
            float randomLaunchForce = UnityEngine.Random.Range(grenadeLaunchForceMin , grenadeLaunchForceMax);
            newGrenade.Launch(launchDir, randomLaunchForce, grenadeTorque);

            grenadeTimer = grenadeCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetTransform.position, groundCheck);
    }
}
