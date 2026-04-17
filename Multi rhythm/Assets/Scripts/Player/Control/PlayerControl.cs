using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float maxSpeed = 5f;
    private Rigidbody2D rb;

    private Vector2 gravityDirection = Vector2.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            gravityDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravityDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gravityDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gravityDirection = Vector2.right;
        }
    }

    void FixedUpdate()
    {
        var speedInDirection = Vector2.Dot(rb.linearVelocity, gravityDirection);

        if (speedInDirection < maxSpeed)
        {
            rb.AddForce(gravityDirection * gravityForce);
        }
    }
}
