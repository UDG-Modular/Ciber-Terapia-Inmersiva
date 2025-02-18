using UnityEngine;
using TMPro;

public class VRKeyboard : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    private TouchScreenKeyboard keyboard;

    public void OpenKeyboard()
    {
        Debug.Log("Opening system keyboard");
        // Ensure the input field is in focus and the keyboard opens.
        keyboard = TouchScreenKeyboard.Open(tmpInputField.text, TouchScreenKeyboardType.Default);
    }

    void Update()
    {
        if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            tmpInputField.text = keyboard.text;
        }
    }
}
