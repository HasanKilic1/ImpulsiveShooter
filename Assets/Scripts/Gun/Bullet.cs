using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletVfx;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float knockBackThrust = 20f;
    [SerializeField] private TrailRenderer trailRenderer;

    private Gun gun;
    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;

    private void OnEnable()
    {
        trailRenderer?.Clear();
    }

    private void OnDisable()
    {
        trailRenderer?.Clear();
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = _fireDirection * _moveSpeed;
    }

    public void Init(Gun gun , Vector2 bulletSpawnPos , Vector2 mousePos)
    {
        this.gun = gun;
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
        ClearTrail();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Instantiate(bulletVfx , transform.position , transform.rotation);

        IHitable hitable = other.gameObject.GetComponent<IHitable>();
        hitable?.TakeHit();

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(_damageAmount , knockBackThrust);

        gun.ReleaseBulletFromPool(this);
    }

    public void ClearTrail() => trailRenderer.Clear();
}