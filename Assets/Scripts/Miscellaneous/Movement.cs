using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;
    private float moveX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    public void SetCurrentXDirection(float x) => moveX = x;

}
