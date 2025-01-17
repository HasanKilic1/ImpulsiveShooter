using System.Collections;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private float disableColliderTime = 1.0f;
    private bool playerOnPlatform = false;
    private Collider2D platformCollider;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        DetectPlayerInput();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            playerOnPlatform = false;
        }
    }

    private void DetectPlayerInput()
    {
        if (!playerOnPlatform) return;

        if(PlayerController.Instance.MoveInput.y < 0)
        {
            StartCoroutine(DisablePlatformColliderRoutine());
        }
    }

    private IEnumerator DisablePlatformColliderRoutine()
    {
        Collider2D[] playerColliders = PlayerController.Instance.GetComponents<Collider2D>();

        foreach (var playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        }

        yield return new WaitForSeconds(disableColliderTime);

        foreach (var playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }
}
