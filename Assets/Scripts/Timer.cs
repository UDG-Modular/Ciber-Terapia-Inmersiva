using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float tiempoPanelActivo = 10f; // Tiempo que el panel permanecerá activo
    private bool panelMostrado = false;
    private bool temporizadorActivo = true;
    private static Timer instancia;

    private GameObject panelInstrucciones; // Panel dentro del jugador

    private void Awake()
    {
        AsignarPanel();
    }

    private void Update()
    {
        if (!temporizadorActivo || panelInstrucciones == null) return;

        if (!panelMostrado && tiempoPanelActivo <= 0)
        {
            DesactivarPanel();
        }
        else
        {
            tiempoPanelActivo -= Time.deltaTime;
        }
    }

    private void AsignarPanel()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player"); // Find the player
        if (jugador != null)
        {
            Transform[] allChildren = jugador.GetComponentsInChildren<Transform>(true); // Get all children (including inactive)
            foreach (Transform child in allChildren)
            {
                if (child.name == "InstructionPanel") // Match by name
                {
                    Debug.Log("Encontrado");
                    panelInstrucciones = child.gameObject;
                    return; // Stop once found
                }
            }
        }
    }


    private void DesactivarPanel()
    {
        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(false);
            panelMostrado = true;
            temporizadorActivo = false; // Detener el temporizador para que no vuelva a activarse
        }
    }
}
