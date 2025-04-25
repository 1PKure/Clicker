using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text creditsText;

    void Start()
    {
        creditsText.text = "Juego desarrollado por\nMatias Pulido\n\npara la materia\nPortabilidad y optimización de\nImage Campus";
    }

    public void BackToGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}