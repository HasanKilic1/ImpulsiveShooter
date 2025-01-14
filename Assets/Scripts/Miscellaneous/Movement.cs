using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;
    private float moveX;
    private bool canMove = true;
    private KnockBack knockBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }

    private void OnEnable()
    {
        knockBack.OnKnockbackStart += CanMoveFalse;
        knockBack.OnKnockbackEnd += CanMoveTrue;
    }

    private void OnDisable()
    {
        knockBack.OnKnockbackStart -= CanMoveFalse;
        knockBack.OnKnockbackEnd -= CanMoveTrue;
    }

    private void FixedUpdate()
    {
        if(canMove)
            Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    public void SetCurrentXDirection(float x) => moveX = x;

    private void CanMoveTrue() => canMove = true;

    private void CanMoveFalse() => canMove = false;
}
