using UnityEngine;
using System.Collections.Generic;

public class FootstepSound : MonoBehaviour
{
    public Terrain terrain;
    public AudioSource audioSource;
    public List<TextureSoundMapping> textureSoundMappings;

    private Dictionary<int, AudioClip> textureSoundMap;
    private TerrainData terrainData;
    private Vector3 terrainPosition;
    private bool isPlayingFootstep = false; // Prevent overlapping sounds
    private Vector3 lastPosition; // Track last position of the capsule

    [System.Serializable]
    public class TextureSoundMapping
    {
        public List<int> textureIndices;
        public AudioClip footstepSound;
    }

    void Start()
    {
        terrainData = terrain.terrainData;
        terrainPosition = terrain.GetPosition();

        textureSoundMap = new Dictionary<int, AudioClip>();
        foreach (var mapping in textureSoundMappings)
        {
            foreach (int textureIndex in mapping.textureIndices)
            {
                textureSoundMap[textureIndex] = mapping.footstepSound;
            }
        }
    }

    void Update()
    {
        // Detect if the capsule has moved and if the footstep sound is not already playing
        if (HasPlayerMoved() && !isPlayingFootstep)
        {
            Debug.Log("Capsule Moved");
            PlayFootstepSound();
        }
    }

    bool HasPlayerMoved()
    {
        // Check if the capsule's position has changed significantly (indicating movement)
        Vector3 currentPosition = transform.position;
        if (Vector3.Distance(currentPosition, lastPosition) > 0.05f) // Threshold for movement detection
        {
            lastPosition = currentPosition; // Update last position
            return true;
        }

        return false;
    }

    void PlayFootstepSound()
    {
        // Prevent overlapping footstep sounds if already playing
        if (isPlayingFootstep) return;

        Vector3 playerPosition = transform.position - terrainPosition;

        int mapX = Mathf.Clamp((int)((playerPosition.x / terrainData.size.x) * terrainData.alphamapWidth), 0, terrainData.alphamapWidth - 1);
        int mapZ = Mathf.Clamp((int)((playerPosition.z / terrainData.size.z) * terrainData.alphamapHeight), 0, terrainData.alphamapHeight - 1);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int dominantTextureIndex = 0;
        float maxMix = 0;
        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            if (splatmapData[0, 0, i] > maxMix)
            {
                maxMix = splatmapData[0, 0, i];
                dominantTextureIndex = i;
            }
        }

        if (textureSoundMap.ContainsKey(dominantTextureIndex))
        {
            isPlayingFootstep = true;
            AudioClip clip = textureSoundMap[dominantTextureIndex];
            audioSource.PlayOneShot(clip);

            // Start the coroutine to reset the flag when the sound finishes
            StartCoroutine(ResetFootstep(clip));
        }
        else
        {
            Debug.LogWarning("No sound mapping found for texture index: " + dominantTextureIndex);
        }
    }

    private System.Collections.IEnumerator ResetFootstep(AudioClip clip)
    {
        // Wait until the audio clip finishes playing
        yield return new WaitForSeconds(clip.length);
        isPlayingFootstep = false; // Reset the flag after the sound finishes
        Debug.Log("Footstep sound finished");
    }
}
