using UnityEngine;
using System;
using System.Collections;

public class KnockBack : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;

    [SerializeField] private float knockbackTime = 0.2f;
    private Vector3 hitDirection;
    private float knockbackThrust;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockBackForce;
        OnKnockbackEnd += StopKnockRoutine;
    }

    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockBackForce;
        OnKnockbackEnd -= StopKnockRoutine;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Vector3 hitDirection , float knockbackThrust)
    {
        this.hitDirection = hitDirection;
        this.knockbackThrust = knockbackThrust;

        OnKnockbackStart?.Invoke();
    }

    private void ApplyKnockBackForce()
    {
        Vector3 difference = (transform.position - hitDirection).normalized * knockbackThrust * rb.mass;
        rb.AddForce(difference , ForceMode2D.Impulse);

        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockbackTime);
        OnKnockbackEnd?.Invoke();
    }

    private void StopKnockRoutine()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
