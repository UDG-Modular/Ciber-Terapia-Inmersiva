using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public TMPro.TMP_InputField userInput;
    public Button nextSceneButton;

    private string defaultText = "Ingresa tu nombre...";

    private void Start()
    {
        nextSceneButton.interactable = false; // Deshabilita el botón al inicio
        userInput.onValueChanged.AddListener(ValidateInput);
        nextSceneButton.onClick.AddListener(SaveAndLoadNextScene);
    }

    private void ValidateInput(string input)
    {
        // Habilita el botón solo si el texto no es vacío ni el predeterminado
        nextSceneButton.interactable = !string.IsNullOrEmpty(input) && input != defaultText;
    }

    private void SaveAndLoadNextScene()
    {
        if (nextSceneButton.interactable)
        {
            UserData.UserName = userInput.text; // Guarda el nombre del usuario
            SceneManager.LoadScene("Main HUB"); // Reemplaza con el nombre de tu escena
        }
    }
}
