using UnityEngine;
using System.Collections;

public class AudioCulling : MonoBehaviour
{
    public Transform player; // Assign the player object
    public float activationDistance = 20f; // Distance to enable audio
    public float fadeDuration = 1.5f; // Time in seconds for fade-in/out
    private AudioSource[] audioSources;

    void Start()
    {
        // Get all Audio Sources in child objects
        audioSources = GetComponentsInChildren<AudioSource>();

        // Start with all sources at volume 0 and disabled
        foreach (AudioSource audio in audioSources)
        {
            audio.volume = 0f;
            audio.enabled = false;
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
            audio.enabled = true; // Enable before fading in
            if (!audio.isPlaying) audio.Play();
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
            audio.enabled = false; // Disable after fading out
        }
    }
}
