using UnityEngine;
using UnityEngine.InputSystem;

public class NaveInput : MonoBehaviour
{
    public float fuerza = 8f;

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
}
