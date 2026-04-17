using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector2 Position => transform.position;

    public float width => transform.localScale.x;
    public float height => transform.localScale.y;
}