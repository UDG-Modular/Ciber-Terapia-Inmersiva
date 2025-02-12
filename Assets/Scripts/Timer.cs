using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float tiempoRestante = 60f; // Tiempo total del temporizador
    private float tiempoPanelActivo = 10f; // Tiempo que el panel permanecerá activo
    private bool panelMostrado = false;
    private bool temporizadorActivo = true;
    private static Timer instancia;

    private GameObject panelTiempoTerminado; // Panel dentro del jugador

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Mantener este objeto al cambiar de escena
            SceneManager.sceneLoaded += OnSceneLoaded; // Detectar cambio de escena
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    private void Start()
    {
        ReasignarPanel(); // Buscar el panel en la escena actual
    }

    private void Update()
    {
        if (!temporizadorActivo || panelTiempoTerminado == null) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
        }
        else if (!panelMostrado)
        {
            tiempoRestante = 0;
            ActivarPanel();
        }
        else
        {
            tiempoPanelActivo -= Time.deltaTime;
            if (tiempoPanelActivo <= 0)
            {
                DesactivarPanel();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReasignarPanel(); // Buscar el panel cuando cambie la escena
    }

    private void ReasignarPanel()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player"); // Find the player
        if (jugador != null)
        {
            Transform[] allChildren = jugador.GetComponentsInChildren<Transform>(true); // Get all children (including inactive)
            foreach (Transform child in allChildren)
            {
                Debug.Log(child);
                if (child.name == "timeOutPanel") // Match by name
                {
                    panelTiempoTerminado = child.gameObject;
                    Debug.Log("Found");
                    return; // Stop once found
                }
            }
        }
    }

    private void ActivarPanel()
    {
        if (panelTiempoTerminado != null)
        {
            panelTiempoTerminado.SetActive(true);
            panelMostrado = true;
        }
    }

    private void DesactivarPanel()
    {
        if (panelTiempoTerminado != null)
        {
            panelTiempoTerminado.SetActive(false);
            panelMostrado = false;
            temporizadorActivo = false; // Detener el temporizador para que no vuelva a activarse
        }
    }
}
