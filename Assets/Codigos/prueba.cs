using UnityEngine;
using UnityEngine.UI; // Para usar la UI

public class WarningDisplay : MonoBehaviour
{
    // Referencia al panel o pantalla de advertencia
    public GameObject timeReachedPanel;
    private float timer = 1200f; // Tiempo de espera en segundos

    void Start()
    {
        // Inicialmente, la pantalla de advertencia esta oculta
        timeReachedPanel.SetActive(false);
    }

    void Update()
    {
        // Reducir el tiempo del temporizador
        timer -= Time.deltaTime;

        // Cuando hayan pasado 20 minutos
        if (timer <= 0f)
        {
            ShowWarning(); // Mostrar la advertencia
        }
    }

    void ShowWarning()
    {
        // Mostrar el panel de advertencia
        timeReachedPanel.SetActive(true);
    }
}
