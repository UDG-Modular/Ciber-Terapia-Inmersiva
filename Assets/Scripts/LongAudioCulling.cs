using UnityEngine;
using System.Collections;

public class LongAudioCulling : MonoBehaviour
{
    public Transform player; // Asigna el jugador
    public float activationDistance = 20f; // Distancia para activar el audio
    public float fadeDuration = 1.5f; // Duración del fade in/out
    private AudioSource[] audioSources;
    private const float clipLength = 1800f; // 30 minutos en segundos

    void Start()
    {
        // Obtener todas las fuentes de audio en los hijos
        audioSources = GetComponentsInChildren<AudioSource>();

        // Iniciar todas las fuentes con volumen 0, desactivadas y con un tiempo aleatorio
        foreach (AudioSource audio in audioSources)
        {
            audio.volume = 0f;
            audio.enabled = false;
            audio.time = GetRandomStartTime(); // Asignar tiempo de inicio aleatorio
        }
    }

    void Update()
    {
        foreach (AudioSource audio in audioSources)
        {
            float distance = Vector3.Distance(player.position, audio.transform.position);
            bool shouldBeActive = distance < activationDistance;

            if (shouldBeActive && !audio.enabled)
            {
                StartCoroutine(FadeAudio(audio, 1f)); // Fade in
            }
            else if (!shouldBeActive && audio.enabled)
            {
                StartCoroutine(FadeAudio(audio, 0f)); // Fade out
            }
        }
    }

    IEnumerator FadeAudio(AudioSource audio, float targetVolume)
    {
        if (targetVolume > 0f)
        {
            audio.enabled = true; // Habilitar antes de hacer fade in
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }

        float startVolume = audio.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }

        audio.volume = targetVolume;

        if (targetVolume == 0f)
        {
            audio.Stop();
            audio.enabled = false; // Deshabilitar después de fade out
        }
    }

    // Genera un punto de inicio aleatorio dentro del clip (0 a 1800 segundos)
    float GetRandomStartTime()
    {
        return Random.Range(0f, clipLength);
    }
}
