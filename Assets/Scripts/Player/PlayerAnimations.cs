using Unity.Cinemachine;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem moveDustVfx;
    [SerializeField] private ParticleSystem jumpVfx;

    [SerializeField] private float tiltAngle = 20f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private Transform playerSpriteTransform;
    [SerializeField] private Transform hatSpriteTransform;
    [SerializeField] private float yLandVelocityCheck = -10f;

    private Vector2 velocityBeforePhysicsUpdate;
    private Rigidbody2D rb;
    private CinemachineImpulseSource impulseSource;

    private void OnEnable()
    {
        PlayerController.OnJump += PlayJumpVfx;
    }

    private void OnDisable()
    {
        PlayerController.OnJump -= PlayJumpVfx;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        DetectMoveDust();
        ApplyTilt();
    }

    private void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(velocityBeforePhysicsUpdate.y < yLandVelocityCheck)
        {
            PlayJumpVfx();
            impulseSource.GenerateImpulse();
        }
    }

    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckGrounded())
        {
            if (!moveDustVfx.isPlaying)
                moveDustVfx.Play();
        }

        else if (moveDustVfx.isPlaying)
        {
            moveDustVfx.Stop();
        }
    }

    private void ApplyTilt()
    {
        float targetAngle;
        if (PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = tiltAngle;
        }
        else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAngle = -tiltAngle;
        }
        else targetAngle = 0f;

        Quaternion currentCharacterRotation = playerSpriteTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(currentCharacterRotation.eulerAngles.x, currentCharacterRotation.eulerAngles.y, targetAngle);

        playerSpriteTransform.rotation = Quaternion.Lerp(currentCharacterRotation, targetRotation, tiltSpeed * Time.deltaTime);

        Quaternion currentHatRotation = hatSpriteTransform.rotation;
        Quaternion hatTargetRotation = Quaternion.Euler(currentHatRotation.eulerAngles.x, currentHatRotation.eulerAngles.y, -targetAngle / 2);

        hatSpriteTransform.rotation = Quaternion.Lerp(currentCharacterRotation, hatTargetRotation, tiltSpeed * Time.deltaTime * 2);
    }

    private void PlayJumpVfx()
    {
        jumpVfx.transform.up = PlayerController.Instance.transform.up;
        jumpVfx.Play();
    }

}
