using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float maxSpeed = 5f;
    private Rigidbody2D rb;

    private Vector2 gravityDirection = Vector2.down;

    private void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            gravityDirection = Vector2.up;
            RotateToGravity();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravityDirection = Vector2.down;
            RotateToGravity();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gravityDirection = Vector2.left;
            RotateToGravity();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gravityDirection = Vector2.right;
            RotateToGravity();
        }
    }

    private void FixedUpdate()
    {
        var speedInDirection = Vector2.Dot(rb.linearVelocity, gravityDirection);

        if (speedInDirection < maxSpeed)
        {
            rb.AddForce(gravityDirection * gravityForce);
        }
    }

    private void RotateToGravity()
    {
        var scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x);
        scale.y = Mathf.Abs(scale.y);

        if (gravityDirection == Vector2.up)
            scale.y *= -1f;

        if (gravityDirection == Vector2.left)
            scale.x *= -1f;

        transform.localScale = scale;
    }
}