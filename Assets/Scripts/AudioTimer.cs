using UnityEngine;

public class AudioTimer: MonoBehaviour
{
    public float tiempoEspera = 900f; // Tiempo antes de reproducir el audio (15 minutos)
    public AudioSource fuenteAudio; // Arrastra aquí el AudioSource deseado en el Inspector

    private bool audioReproducido = false;
    private float temporizador;

    void Start()
    {
        if (fuenteAudio == null)
        {
            Debug.LogError("No se asignó ningún AudioSource.");
        }

        temporizador = tiempoEspera;
    }

    void Update()
    {
        if (audioReproducido || fuenteAudio == null)
            return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0f)
        {
            fuenteAudio.Play();
            audioReproducido = true;
        }
    }
}
