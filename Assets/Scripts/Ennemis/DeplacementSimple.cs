using UnityEngine;

public class DeplacementSimple : MonoBehaviour
{
    public Vector2 direction;
    public float vitesse;
    Rigidbody2D rigid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.AddForce(direction * vitesse);
        rigid.linearVelocityX = Mathf.Clamp(rigid.linearVelocityX, -18, 18);

        if (transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
    }
}
