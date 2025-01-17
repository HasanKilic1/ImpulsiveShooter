using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public static Action OnBeep;
    public static Action OnExplosion;

    [SerializeField] private float travelTime = 4f;
    [SerializeField] private float beepTime = 2f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage = 5;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material beepMaterial;
    [SerializeField] private GameObject grenadeVfx;

    private float timeElapsed;
    private bool exploded = false;
    
    private CinemachineImpulseSource impulseSource;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (exploded)
        {
            return;
        }

        timeElapsed += Time.deltaTime;
        if(timeElapsed >= travelTime)
        {
            exploded = true;
            StartCoroutine(BeepRoutine());
        }
    }

    public void Launch(Vector2 forceDirection , float launchForce , float torqueForce)
    {
        rb.AddForce(forceDirection * launchForce, ForceMode2D.Impulse);
        rb.AddTorque(torqueForce);
    }

    private IEnumerator BeepRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            OnBeep?.Invoke();
            StartCoroutine(MaterialRoutine(beepTime / 3f));
            yield return new WaitForSeconds(beepTime / 3f);
        }
        Explode();
    }

    private IEnumerator MaterialRoutine(float interval)
    {
        SetMaterial(beepMaterial);
        yield return new WaitForSeconds(interval / 2);
        SetMaterial(defaultMaterial);
    }

    private void SetMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    private void Explode()
    {
        OnExplosion?.Invoke();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (var coll in colliders)
        {
            if(coll.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage, 10);
            }
        }
        impulseSource.GenerateImpulse();
        Instantiate(grenadeVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
