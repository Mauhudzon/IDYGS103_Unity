using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;

// Herramienta de apoyo: renombra las escenas segun la lista de entrega.
// Uso: menu  Herramientas > Renombrar escenas (lista de entrega)
// Usa AssetDatabase.RenameAsset, que conserva el GUID: las escenas, sus objetos
// y las Build Settings siguen funcionando. Puedes borrar este archivo despues.
public static class RenombrarEscenas
{
    static readonly string[,] Cambios =
    {
        { "SampleScene",             "1-SampleScene (objetos simples)" },
        { "Scene1",                  "2-Scene 1 (Físicas básicas)" },
        { "Plinko",                  "3-Plinko" },
        { "Scene2",                  "6-Nave" },
        { "Escena01_Transform",      "9-clase 5 Ejercicio 1 — Movimiento directo con Transform" },
        { "Escena02_AddForce",       "10-clase 5 Ejercicio 2 — Impulsos y fuerzas con Rigidbody2D" },
        { "Escena03_LinearVelocity", "11-clase 5 Ejercicio 3 — Velocidad, gravedad y colisiones" },
        { "Escena01_InputMovement",  "13-Ejercicio 2 — Mover un personaje con el New Input System" },
        { "Escena02_InputPhysics",   "14-Ejercicio 3 — Fuerzas, impulso y cambio de color" },
        { "Reto_Input_Equilibrio",   "15-Reto final — Migrar la nave de equilibrio" },
    };

    [MenuItem("Herramientas/Renombrar escenas (lista de entrega)")]
    public static void Ejecutar()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Renombrar escenas", "Deten el modo Play antes de usar esta herramienta.", "OK");
            return;
        }

        // Guardar la escena abierta antes de renombrar
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        var resumen = new StringBuilder("Renombrado de escenas:\n\n");
        int hechos = 0;

        for (int i = 0; i < Cambios.GetLength(0); i++)
        {
            string actual = "Assets/Scenes/" + Cambios[i, 0] + ".unity";
            string nuevo = Cambios[i, 1];
            string rutaNueva = "Assets/Scenes/" + nuevo + ".unity";

            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(rutaNueva) != null)
            {
                resumen.AppendLine("= Ya estaba: " + nuevo);
                continue;
            }
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(actual) == null)
            {
                resumen.AppendLine("? No encontre: " + Cambios[i, 0]);
                continue;
            }

            string error = AssetDatabase.RenameAsset(actual, nuevo);
            if (string.IsNullOrEmpty(error))
            {
                resumen.AppendLine("OK " + Cambios[i, 0] + "  ->  " + nuevo);
                hechos++;
            }
            else
            {
                resumen.AppendLine("ERROR " + Cambios[i, 0] + ": " + error);
            }
        }

        AssetDatabase.SaveAssets();
        resumen.AppendLine("\nEscenas renombradas: " + hechos);
        UnityEngine.Debug.Log(resumen.ToString());
        EditorUtility.DisplayDialog("Renombrar escenas", resumen.ToString(), "OK");
    }
}
