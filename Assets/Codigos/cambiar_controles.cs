using UnityEngine;
using UnityEngine.UI;

public class CambiarControles : MonoBehaviour
{
    [SerializeField] private Toggle opcionesToggle;

    private void Start()
    {
        // Asegurarse de que el evento este conectado
        if (opcionesToggle != null)
        {
            opcionesToggle.onValueChanged.AddListener(CambiarModoControles);
        }
        else
        {
            Debug.LogError("Toggle no asignado en el inspector.");
        }
    }

    private void CambiarModoControles(bool esMetaQuest)
    {
        if (esMetaQuest)
        {
            ActivarControlesMetaQuest();
        }
        else
        {
            ActivarControlesXbox();
        }
    }

    private void ActivarControlesMetaQuest()
    {
        Debug.Log("Se activaron los controles de Meta Quest 2.");
        // Aqui se implementan las configuraciones para los controles de Meta Quest.

    }

    private void ActivarControlesXbox()
    {
        Debug.Log("Se activaron los controles de Xbox.");
        // Aqui se implementan las configuraciones para los controles de Xbox.

    }
}