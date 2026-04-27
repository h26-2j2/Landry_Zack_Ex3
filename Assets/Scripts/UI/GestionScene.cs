using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionScene : MonoBehaviour
{
    public string sceneIntro = "EcranTitre";
    public string sceneJeu = "Test-DesignNiveau";

    public void DemarrerJeu()
    {
        SceneManager.LoadScene(sceneJeu);
    }
}
