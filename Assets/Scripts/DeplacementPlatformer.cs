using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementPlatformer : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    [Header("Actions d'entrée")]
    public InputAction actionMarche;
    public InputAction actionSaut;
    public InputAction actionTir;

    [Header("Déplacement horizontal")]
    public float vitesse = 5f;
    public float vitesseXActuelle;
    float inputMarche;

    [Header("Saut")]
    public bool estAuSol = false;
    public bool estEnSaut = false;
    public float forceSaut = 5f;

    [Header("Tir")]
    public float delaiTirMin = 1f;
    public float tempsEntreTir = 0f;
    public GameObject prefabProjectile;
    public Transform positionProjectile;
    public float directionProjectile;
    public float vitesseProjectile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        actionMarche.Enable();
        actionSaut.Enable();
        actionTir.Enable();
    }

    private void OnDisable()
    {
        actionMarche.Disable();
        actionSaut.Disable();
        actionTir.Disable();

    }

    private void Update()
    {

        // Récupère l'axe de promenade
        inputMarche = actionMarche.ReadValue<float>();

        if (inputMarche < 0)
        {
            sr.flipX = true;
            directionProjectile = -1;

            Vector2 nouvellePositionProjectile = positionProjectile.localPosition;
            nouvellePositionProjectile.x = -1.5f;
            positionProjectile.localPosition = nouvellePositionProjectile;
        }
        else if (inputMarche > 0)
        {
            sr.flipX = false;

            directionProjectile = 1;
            Vector2 nouvellePositionProjectile = positionProjectile.localPosition;
            nouvellePositionProjectile.x = 1.5f;
            positionProjectile.localPosition = nouvellePositionProjectile;
        }

        if (actionSaut.WasPressedThisFrame() && estAuSol == true)
        {
            estEnSaut = true;
        }
        else
        {
            estEnSaut = false;
        }


        if (tempsEntreTir > 0)
        {
            tempsEntreTir -= Time.deltaTime;
        }

        if (actionTir.WasPressedThisFrame() && tempsEntreTir <= 0f)
        {
            tempsEntreTir = delaiTirMin;
            anim.SetTrigger("tir");

            GameObject clone = Instantiate(prefabProjectile, positionProjectile.position, positionProjectile.rotation);
            clone.GetComponent<Projectile>().direction = directionProjectile;
            clone.GetComponent<Projectile>().vitesse = vitesseProjectile;

        }

        anim.SetFloat("vitesseMarche", Mathf.Abs(rb.linearVelocityX));
        anim.SetBool("estAuSol", estAuSol);
    }

    private void FixedUpdate()
    {

        if (inputMarche != 0)
        {
            rb.linearVelocityX = vitesse * inputMarche;
        }

        // Détection du sol
        estAuSol = Physics2D.Raycast(rb.position, Vector2.down, 1f, LayerMask.GetMask("Obstacle", "Default"));
        // Logique de contrôle de saut
        if (estEnSaut)
        {
            estEnSaut = false;
            rb.AddForce(Vector2.up * forceSaut, ForceMode2D.Impulse);
        }
    }

}
