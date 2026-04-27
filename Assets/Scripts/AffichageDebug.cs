using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AffichageDebug : MonoBehaviour
{
    private TMP_Text texte;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texte = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        texte.text = $"Scène : {SceneManager.GetActiveScene().name}\nTemps : {Time.time.ToString("00.0")}s";
    }
}
