using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CambioColor))]
public class PlataformaQueCae : MonoBehaviour
{
    public float retraso = 0.6f;            // tiempo antes de caer
    public float tiempoReaparecer = 4f;     // tiempo hasta volver a su lugar
    public Color colorNormal = new Color(1f, 0.6f, 0.2f);   // naranja
    public Color colorAviso = Color.red;

    private Rigidbody2D rb;
    private CambioColor cambioColor;
    private Vector3 posicionInicial;
    private bool activada;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cambioColor = GetComponent<CambioColor>();
        posicionInicial = transform.position;

        rb.bodyType = RigidbodyType2D.Kinematic;               // quieta mientras nadie la pisa
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // que no gire al caer
    }

    void Start()
    {
        cambioColor.CambiarColor(colorNormal);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activada && collision.gameObject.CompareTag("Player"))
        {
            activada = true;
            cambioColor.CambiarColor(colorAviso);
            Invoke(nameof(Caer), retraso);
        }
    }

    void Caer()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;  // ahora la gravedad la afecta
        rb.gravityScale = 2f;
        Invoke(nameof(Reiniciar), tiempoReaparecer);
    }

    void Reiniciar()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        transform.position = posicionInicial;   // regresa a su lugar
        activada = false;
        cambioColor.CambiarColor(colorNormal);
    }
}