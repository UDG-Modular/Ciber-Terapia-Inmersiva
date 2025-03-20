using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerFlight : MonoBehaviour
{
    [SerializeField] private float flightSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float doublePressTime = 0.3f;
    [SerializeField] private float maxFlightHeight = 20f; // Altura máxima de vuelo
    [SerializeField] private AudioClip flightOnSound;
    [SerializeField] private AudioClip flightOffSound;
    [SerializeField] private AudioSource audioSource;

    private CharacterController characterController;
    [SerializeField] private ActionBasedContinuousMoveProvider moveProvider;
    public static bool IsFlying { get; private set; } = false;

    private float lastPressTime = -1f;
    private float initialY; // Guarda la altura inicial al volar

    public InputActionProperty rightPrimaryButton;
    public InputActionProperty rightSecondaryButton;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleFlightInput();
        ApplyGravity();
    }

    private void HandleFlightInput()
    {
        if (rightPrimaryButton.action.WasPressedThisFrame())
        {
            if (Time.time - lastPressTime <= doublePressTime)
            {
                ToggleFlight();
                lastPressTime = -1f; // Evita múltiples activaciones
            }
            else
            {
                lastPressTime = Time.time;
            }
        }

        if (IsFlying)
        {
            gravity = 0;
            Vector3 flightVelocity = Vector3.zero;

            if (rightPrimaryButton.action.IsPressed()) // Subir
            {
                if (transform.position.y < initialY + maxFlightHeight) // Límite de altura
                {
                    flightVelocity += Vector3.up * flightSpeed;
                }
            }
            if (rightSecondaryButton.action.IsPressed()) // Bajar
            {
                flightVelocity += Vector3.down * flightSpeed;
            }

            characterController.Move(flightVelocity * Time.deltaTime);
        }
    }

    private void ToggleFlight()
    {
        IsFlying = !IsFlying;
        audioSource.PlayOneShot(IsFlying ? flightOnSound : flightOffSound);
        Debug.Log(IsFlying ? "Vuelo Activado" : "Vuelo Desactivado");

        if (IsFlying)
        {
            moveProvider.moveSpeed = 10;
            initialY = transform.position.y; // Guarda la altura al empezar a volar
        }
        if (!IsFlying)
        {
            moveProvider.moveSpeed = 5;
        }
    }

    private void ApplyGravity()
    {
        Debug.Log("Fly: " + IsFlying);
        Debug.Log("Grounded: " + characterController.isGrounded);
        gravity = 9.81f;
        if (!IsFlying && !characterController.isGrounded)
        {
            Debug.Log("Gravity enabled");
            characterController.Move(Vector3.down * gravity * Time.deltaTime);
        }
    }
}
