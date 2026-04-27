using UnityEngine;

public class GestionVieEnnemi : MonoBehaviour
{
    //Variables privées
    Animator animator;
    Collider2D collider;
    AudioSource audioSource;

    // Variables publiques
    public bool estMort;
    public AudioClip sonMort;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponentInChildren<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile" && estMort == false)
        {
            Mourir();
        }
    }

    /**
    * Fonction déclenchée lors de la mort de l'ennemi
    */
    void Mourir()
    {
        audioSource.PlayOneShot(sonMort);
        estMort = true;
        collider.enabled = false;
        animator.SetTrigger("estMort");
    }
}
