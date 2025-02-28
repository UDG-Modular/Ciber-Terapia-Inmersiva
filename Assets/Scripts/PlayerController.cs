using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movementInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    public bool IsMoving()
    {
        return movementInput.magnitude > 0.1f; // Detecta si el jugador se está moviendo
    }
}
