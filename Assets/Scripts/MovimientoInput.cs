using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoInput : MonoBehaviour
{
    public float velocidad = 5f;

    private Vector2 movimiento;

    public void OnMove(InputValue value)
    {
        movimiento = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 direccion = new Vector3(
            movimiento.x,
            movimiento.y,
            0f
        );

        transform.Translate(
            direccion * velocidad * Time.deltaTime
        );
    }
}
