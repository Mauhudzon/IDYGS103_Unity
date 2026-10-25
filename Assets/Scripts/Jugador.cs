using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CambioColor))]
public class Jugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 6f;
    public float fuerzaSalto = 12f;

    [Header("Colores")]
    public Color colorSuelo = Color.gray;
    public Color colorAire = Color.cyan;
    public Color colorMeta = Color.green;

    private Rigidbody2D rb;
    private CambioColor cambioColor;

    private float entradaHorizontal;
    private bool enSuelo;
    private bool tocandoSuelo;      // lo marcan las colisiones
    private bool nivelCompletado;
    private Vector3 puntoInicio;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cambioColor = GetComponent<CambioColor>();
        puntoInicio = transform.position;   // aquí reaparece
    }

    void Start()
    {
        cambioColor.CambiarColor(colorSuelo);
    }

    // Update: leer el teclado (Clase 04)
    void Update()
    {
        if (nivelCompletado) return;

        entradaHorizontal = Input.GetAxisRaw("Horizontal");

        // Salto: un solo impulso por pulsación (Clase 05)
        if (Input.GetKey(KeyCode.Space) && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            enSuelo = false;
        }
    }

    // FixedUpdate: mover con física (Clase 05)
    void FixedUpdate()
    {
        // Actualizar si estamos en el suelo con lo que reportaron las colisiones
        bool estabaEnSuelo = enSuelo;
        enSuelo = tocandoSuelo;
        tocandoSuelo = false;

        if (nivelCompletado)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // Correr: fijamos la velocidad X y conservamos la Y (gravedad, saltos)
        rb.linearVelocity = new Vector2(entradaHorizontal * velocidad, rb.linearVelocity.y);

        // Cambiar el color solo cuando cambia el estado
        if (enSuelo != estabaEnSuelo)
        {
            cambioColor.CambiarColor(enSuelo ? colorSuelo : colorAire);
        }
    }

    // Se llama en cada paso de física mientras tocamos algo
    void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            // La normal apunta "hacia afuera" de lo que tocamos.
            // Si apunta hacia arriba, eso que tocamos está DEBAJO: es suelo.
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                tocandoSuelo = true;
                return;
            }
        }
    }

    // Se llama una vez, al empezar un choque
    void OnCollisionEnter2D(Collision2D collision)
    {
        // collision.collider = el collider exacto que tocamos
        // (funciona aunque la meta sea hija de otro objeto con Rigidbody)
        if (collision.collider.CompareTag("Peligro"))
        {
            Reaparecer();
        }
        else if (collision.collider.CompareTag("Meta"))
        {
            Ganar();
        }
    }

    // Por si la meta o los peligros tienen el collider marcado como "Is Trigger"
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Peligro"))
        {
            Reaparecer();
        }
        else if (other.CompareTag("Meta"))
        {
            Ganar();
        }
    }

    void Reaparecer()
    {
        // Teletransporte: aquí sí conviene mover directo, sin física
        transform.position = puntoInicio;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("¡Caíste! Vuelves al inicio.");
    }

    void Ganar()
    {
        if (nivelCompletado) return;

        nivelCompletado = true;
        cambioColor.CambiarColor(colorMeta);
        Debug.Log("¡Nivel completado!");
    }
}