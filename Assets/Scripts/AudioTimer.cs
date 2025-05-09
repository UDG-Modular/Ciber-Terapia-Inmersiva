using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioTimer : MonoBehaviour
{
    public float tiempoEspera; // Tiempo antes de reproducir el audio (15 minutos)
    public AudioSource fuenteAudio; // Asigna el AudioSource desde el Inspector
    public Sprite[] Background; // Sprites de la animación
    public GameObject respiracion; // Imagen a animar
    public AudioClip nuevoAudio; // Nuevo audio a reproducir tras la animación

    private bool audioReproducido = false;
    private float temporizador;
    private Image respiracionImage;

    void Start()
    {
        respiracion.SetActive(false);
        respiracionImage = respiracion.GetComponent<Image>();

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
            StartCoroutine(EsperarFinAudio());
            audioReproducido = true;
        }
    }

    IEnumerator EsperarFinAudio()
    {
        // Espera a que termine el audio
        while (fuenteAudio.isPlaying)
        {
            yield return null;
        }

        // Inicia animación de respiración
        StartCoroutine(TimeWithinFrames());
    }

    IEnumerator TimeWithinFrames()
    {
        int totalFrames = Background.Length - 1;
        float duration = 2f; // Duración de inhalación o exhalación
        int cycles = 3;

        respiracion.SetActive(true);

        for (int j = 0; j < cycles; j++)
        {
            // Exhalar
            yield return StartCoroutine(SmoothAnimation(1, 0, totalFrames, duration, true));

            respiracionImage.sprite = Background[0];
            yield return new WaitForSeconds(1f);

            // Inhalar
            yield return StartCoroutine(SmoothAnimation(0, 1, totalFrames, duration, false));

            respiracionImage.sprite = Background[totalFrames];
            yield return new WaitForSeconds(1f);
        }

        respiracion.SetActive(false);

        // Asigna y reproduce el nuevo audio
        if (nuevoAudio != null)
        {
            fuenteAudio.clip = nuevoAudio;
            fuenteAudio.Play();
        }
        else
        {
            Debug.LogWarning("No se asignó un nuevo audio para reproducir.");
        }
    }

    IEnumerator SmoothAnimation(float start, float end, int maxFrame, float duration, bool easeOut)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float easedT = easeOut
                ? Mathf.Sin(t * Mathf.PI * 0.5f)
                : 0.5f - 0.5f * Mathf.Cos(t * Mathf.PI);

            float interpolated = Mathf.Lerp(start, end, easedT);
            int frameIndex = Mathf.Clamp(Mathf.RoundToInt(interpolated * maxFrame), 0, maxFrame);

            respiracionImage.sprite = Background[frameIndex];
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
