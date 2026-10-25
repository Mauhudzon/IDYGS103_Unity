using UnityEngine;

[RequireComponent(typeof(CambioColor))]
public class PlataformaTemporal : MonoBehaviour
{
    public float tiempoVisible = 1.5f;    // desde que la pisas hasta que desaparece
    public float tiempoReaparecer = 3f;   // cuánto tarda en volver
    public Color colorInicial = Color.green;
    public Color colorFinal = Color.red;

    private CambioColor cambioColor;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool activada;
    private float tiempoPisada;

    void Awake()
    {
        cambioColor = GetComponent<CambioColor>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        cambioColor.CambiarColor(colorInicial);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!activada && collision.gameObject.CompareTag("Player"))
        {
            activada = true;
            tiempoPisada = Time.time;
        }
    }

    void Update()
    {
        if (!activada || !spriteRenderer.enabled) return;

        // t va de 0 (recién pisada) a 1 (se acabó el tiempo)
        float t = (Time.time - tiempoPisada) / tiempoVisible;

        if (t < 1f)
        {
            // Mezcla gradual entre los dos colores
            cambioColor.CambiarColor(Color.Lerp(colorInicial, colorFinal, t));
        }
        else
        {
            Desaparecer();
        }
    }

    void Desaparecer()
    {
        spriteRenderer.enabled = false;   // ya no se ve
        col.enabled = false;              // ya no se puede pisar
        Invoke(nameof(Reaparecer), tiempoReaparecer);
    }

    void Reaparecer()
    {
        spriteRenderer.enabled = true;
        col.enabled = true;
        activada = false;
        cambioColor.CambiarColor(colorInicial);
    }
}