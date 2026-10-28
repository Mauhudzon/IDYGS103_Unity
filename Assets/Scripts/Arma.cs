using UnityEngine;

public class Arma : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.isTrigger)
        {
            return;
        }
        
        if (other.transform == transform.parent)
        {
            return;
        }

        if (other.name.StartsWith("Jugador"))
        {
            Time.timeScale = 0f;
        }
    }
}
