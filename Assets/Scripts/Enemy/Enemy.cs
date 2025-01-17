using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour , IDamageable
{
    private Movement movement;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;

    private Rigidbody2D rigidBody;
    private ColorChanger colorChanger;
    private KnockBack knockBack;
    private Flash flash;
    private Health health;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        colorChanger = GetComponent<ColorChanger>();
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
        health = GetComponent<Health>();
    }

    private void Start() {
        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(RandomJumpRoutine());
    }

    public void Init(Color color)
    {
        colorChanger.SetDefaultColor(color);
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            float _currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1
            movement.SetCurrentXDirection(_currentDirection);
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJumpRoutine() 
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int damageAmount , float knockbackThrust)
    {
        health.TakeDamage(damageAmount);
        knockBack.GetKnockedBack(PlayerController.Instance.transform.position, knockbackThrust);
    }

    public void TakeHit()
    {
        flash.StartFlash();
    }
}
