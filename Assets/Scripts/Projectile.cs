using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public float vitesse = 4f;
    public float direction = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        if (direction < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        rb.linearVelocityX = vitesse * direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

}
