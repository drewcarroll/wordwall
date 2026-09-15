using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = move * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cat"))
        {
            GameManager.Instance.Lose("The cat caught you!");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Exit"))
        {
            GameManager.Instance.Win();
        }
        else if (collision.gameObject.CompareTag("Lava"))
        {
            GameManager.Instance.Lose("You touched the lava!");
        }
    }
}
