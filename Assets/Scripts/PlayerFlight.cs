using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlight : MonoBehaviour
{
    [SerializeField] private float flightSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float doublePressTime = 0.3f;
    [SerializeField] private AudioClip flightOnSound; // Sonido al activar
    [SerializeField] private AudioClip flightOffSound; // Sonido al desactivar
    private AudioSource audioSource;

    private CharacterController characterController;
    private bool isFlying = false;
    private float lastPressTime = 0f;

    public InputActionProperty rightPrimaryButton;
    public InputActionProperty rightSecondaryButton;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleFlightInput();
        ApplyGravity();
    }

    private void HandleFlightInput()
    {
        if (rightPrimaryButton.action.IsPressed())
        {
            if (Time.time - lastPressTime <= doublePressTime)
            {
                ToggleFlight();
            }
            lastPressTime = Time.time;
        }

        if (isFlying)
        {
            if (rightPrimaryButton.action.IsPressed())
            {
                characterController.Move(Vector3.up * flightSpeed * Time.deltaTime);
            }
            if (rightSecondaryButton.action.IsPressed())
            {
                characterController.Move(Vector3.down * flightSpeed * Time.deltaTime);
            }
        }
    }

    private void ToggleFlight()
    {
        isFlying = !isFlying;
        if (isFlying)
        {
            audioSource.PlayOneShot(flightOnSound);
            Debug.Log("Vuelo Activado");
        }
        else
        {
            audioSource.PlayOneShot(flightOffSound);
            Debug.Log("Vuelo Desactivado");
        }
    }

    private void ApplyGravity()
    {
        if (!isFlying && !characterController.isGrounded)
        {
            characterController.Move(Vector3.down * gravity * Time.deltaTime);
        }
    }
}
