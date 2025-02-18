using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public InputField userInput;
    public Button nextSceneButton;

    private void Start()
    {
        nextSceneButton.onClick.AddListener(SaveAndLoadNextScene);
    }

    private void SaveAndLoadNextScene()
    {
        if (!string.IsNullOrEmpty(userInput.text))
        {
            UserData.UserName = userInput.text; // Store username
        }

        SceneManager.LoadScene("NextScene"); // Replace with your scene name
    }
}
