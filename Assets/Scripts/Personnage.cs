using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Personnage : MonoBehaviour
{
    [Header("Actions")]
    public InputAction actionMarche;
    public InputAction actionSaut;
    public InputAction actionTir;
    public InputAction actionDash; // ✅ AJOUTÉ

    [Header("Déplacement horizontal")]
    float inputDeplacement;
    public float vitesseDeplacement;

    [Header("Saut")]
    bool inputSaut;
    public float forceSaut;
    public bool estAuSol;
    public float direction = 1;
    public LayerMask coucheSol;

    [Header("Tir")]
    bool inputTir;
    public GameObject prefabProjectile;
    public Transform prefabPosition;
    public float timerTir = 0;
    public float timerTirMax = 2;

    [Header("Dash")]
    public float forceDash = 15f;        // ✅ AJOUTÉ
    bool isDashing = false;              // ✅ AJOUTÉ
    public float cooldownDash = 0.5f;    // ✅ AJOUTÉ
    float timerCooldownDash = 0f;        // ✅ AJOUTÉ
    bool inputDash;                      // ✅ AJOUTÉ

    [Header("Sons")]
    public AudioClip sonSaut;
    public AudioClip sonTir;

    [Header("Composants")]
    Rigidbody2D rb;
    AudioSource audioSource;
    public Animator animator;
    public SpriteRenderer sr;

    private void OnEnable()
    {
        actionMarche.Enable();
        actionSaut.Enable();
        actionTir.Enable();
        actionDash.Enable(); // ✅ AJOUTÉ
    }

    private void OnDisable()
    {
        actionMarche.Disable();
        actionSaut.Disable();
        actionTir.Disable();
        actionDash.Disable(); // ✅ AJOUTÉ
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        inputDeplacement = actionMarche.ReadValue<float>();
        inputSaut = actionSaut.WasPressedThisFrame();
        inputTir = actionTir.WasPressedThisFrame();
        inputDash = actionDash.WasPressedThisFrame(); // ✅ AJOUTÉ

        estAuSol = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, coucheSol);
        Debug.DrawRay(transform.position, Vector3.down * 0.4f, Color.orange);

        animator.SetFloat("vitesse", Mathf.Abs(rb.linearVelocityX));
        animator.SetBool("estEnSaut", estAuSol == false);

        // ✅ AJOUTÉ - Gestion du cooldown du dash
        if (timerCooldownDash > 0)
        {
            timerCooldownDash -= Time.deltaTime;
            if (timerCooldownDash <= 0)
            {
                isDashing = false;
            }
        }

        // ✅ AJOUTÉ - Déclenchement de l'animation du dash
        if (inputDash && !isDashing)
        {
            animator.SetTrigger("player-slide");
        }

        if (inputDeplacement < 0)
        {
            direction = -1;
            sr.flipX = true;
            Vector2 positionProjectile = prefabPosition.localPosition;
            positionProjectile.x = -1;
            prefabPosition.localPosition = positionProjectile;
        }
        else if (inputDeplacement > 0)
        {
            direction = 1;
            sr.flipX = false;
            Vector2 positionProjectile = prefabPosition.localPosition;
            positionProjectile.x = 1;
            prefabPosition.localPosition = positionProjectile;
        }

        if (inputTir && timerTir <= 0)
        {
            timerTir = timerTirMax;
            GameObject clone = Instantiate(prefabProjectile, prefabPosition.position, prefabPosition.rotation);
            clone.GetComponent<Projectile>().direction = direction;
            clone.SetActive(true);
            audioSource.PlayOneShot(sonTir);
            animator.SetTrigger("tir");
        }

        if (timerTir > 0)
        {
            timerTir -= Time.deltaTime;
            timerTir = Mathf.Max(timerTir, 0);
        }
    }

    private void FixedUpdate()
    {
        // Bloque le déplacement normal pendant le dash
        if (!isDashing)
        {
            if (inputDeplacement != 0)
            {
                rb.linearVelocityX = inputDeplacement * vitesseDeplacement;
            }
        }

        if (inputSaut && estAuSol)
        {
            rb.AddForce(Vector2.up * forceSaut, ForceMode2D.Impulse);
            audioSource.PlayOneShot(sonSaut);
        }

        if (inputDash && !isDashing)
        {
            rb.linearVelocityX = 0f; // Reset avant l'impulsion
            rb.AddForce(new Vector2(direction * forceDash, 0f), ForceMode2D.Impulse);
            isDashing = true;
            timerCooldownDash = cooldownDash;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lave")
        {
            Scene sceneActuelle = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sceneActuelle.name);
        }
    }
}