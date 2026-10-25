using UnityEngine;

public class Trampolin : MonoBehaviour
{
    public float fuerzaRebote = 20f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Solo si el jugador viene desde ARRIBA
        if (collision.transform.position.y > transform.position.y)
        {
            // GetComponent también funciona sobre OTRO objeto
            Rigidbody2D rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();
            rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, fuerzaRebote);
        }
    }
}