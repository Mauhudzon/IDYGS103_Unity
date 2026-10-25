using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoFisicoInput : MonoBehaviour
{
    public float fuerza = 8f;
    public float fuerzaImpulso = 4f;

    private Rigidbody2D rb;
    private Vector2 movimiento;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        movimiento = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        rb.AddForce(movimiento * fuerza);
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            rb.AddForce(
                Vector2.up * fuerzaImpulso,
                ForceMode2D.Impulse
            );
        }
    }
}