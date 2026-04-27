using UnityEngine;

public class Patrouille : MonoBehaviour
{
    // Variables publiques
    public GameObject point1;
    public GameObject point2;
    public float vitesse;
    public float distanceMin;

    // Variables privées
    float distance;
    GameObject cibleActuelle;
    SpriteRenderer spriteRenderer;
    GestionVieEnnemi gestionVieEnnemi;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cibleActuelle = point1;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gestionVieEnnemi = GetComponent<GestionVieEnnemi>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // On peut bouger si on n'est pas mort
        if (gestionVieEnnemi.estMort == false)
        {
            //On vérifie la distance avec la cible
            distance = Vector2.Distance(
                transform.localPosition,
                cibleActuelle.transform.localPosition
            );

            //Si on est rendu à la cible, on change de cible
            if (cibleActuelle == point1 && distance <= distanceMin)
            {
                cibleActuelle = point2;
            }
            else if (cibleActuelle == point2 && distance <= distanceMin)
            {
                cibleActuelle = point1;
            }

            //On tourne le sprite si la cible est à gauche ou à droite
            spriteRenderer.flipX = transform.localPosition.x < cibleActuelle.transform.localPosition.x;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    //On applique la physique dans le FixedUpdate pour éviter des comportements irréguliers dans le mouvement
    void FixedUpdate()
    {
        // On peut bouger si on n'est pas mort
        if (gestionVieEnnemi.estMort == false)
        {
            //On se dirige vers la cible en question
            Vector3 direction = cibleActuelle.transform.localPosition - transform.localPosition;
            rb.linearVelocity = direction * vitesse;
        }

    }
}
