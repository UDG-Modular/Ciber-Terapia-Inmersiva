using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] private float teleportHeight = 5f; // Altura para teletransportar
    [SerializeField] private float cooldownTime = 2f;   // Tiempo de espera antes de permitir otro teletransporte
    private float nextTeleportTime = 0f; // Tiempo para el próximo teletransporte permitido

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= nextTeleportTime)
        {
            Vector3 playerPosition = other.transform.position;

            // Raycast para detectar el terreno debajo
            if (Physics.Raycast(playerPosition + Vector3.up * 40f, Vector3.down, out RaycastHit hit, 50f))
            {
                other.transform.position = new Vector3(playerPosition.x, hit.point.y + teleportHeight, playerPosition.z);
                nextTeleportTime = Time.time + cooldownTime; // Iniciar cooldown
            }
            else
            {
                Debug.LogWarning("Terreno no encontrado debajo del jugador!");
            }
        }
    }
}
