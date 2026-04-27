using UnityEngine;

public class DecalagePlateformeMobile : MonoBehaviour
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        float decalageAnim = Random.Range(0f, 1f); // valeur entre 0 et 1
        animator.Play("plateformeMobile@idle", 0, decalageAnim); //Permet de s'assurer que les éléments qui ont le même animator ne commence pas tous au même endroit dans l'animation
    }


}
