using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Herramienta de apoyo para la Actividad 8.
// Uso: menu  Herramientas > Actividad 8: Crear picos, vacio y revisar meta
// Trabaja sobre la escena ABIERTA. Puedes borrar este archivo cuando ya no lo necesites.
public static class HerramientasActividad8
{
    [MenuItem("Herramientas/Actividad 8: Crear picos, vacio y revisar meta")]
    public static void Ejecutar()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Actividad 8", "Deten el modo Play antes de usar esta herramienta.", "OK");
            return;
        }

        var resumen = new StringBuilder("ACTIVIDAD 8 - Resultado de la revision:\n");

        // ---------- Tags ----------
        AgregarTag("Peligro", resumen);
        AgregarTag("Meta", resumen);

        // ---------- Jugador ----------
        var jugador = Object.FindFirstObjectByType<Jugador>();
        if (jugador == null)
        {
            resumen.AppendLine("- (!) No hay ningun objeto con el script Jugador en la escena.");
        }
        else if (!jugador.CompareTag("Player"))
        {
            Undo.RecordObject(jugador.gameObject, "Tag Player");
            jugador.gameObject.tag = "Player";
            resumen.AppendLine("- Jugador '" + jugador.name + "': NO tenia Tag Player. Ya se asigno.");
        }
        else
        {
            resumen.AppendLine("- Jugador '" + jugador.name + "': Tag Player correcto.");
        }

        // ---------- Meta ----------
        GameObject meta = BuscarMeta();
        if (meta == null)
        {
            resumen.AppendLine("- (!) No encontre la meta. Crea un objeto llamado 'Meta' con Box Collider 2D y vuelve a usar la herramienta.");
        }
        else
        {
            if (!meta.CompareTag("Meta"))
            {
                Undo.RecordObject(meta, "Tag Meta");
                meta.tag = "Meta";
                resumen.AppendLine("- Meta '" + meta.name + "': NO tenia Tag Meta. Ya se asigno.");
            }
            else
            {
                resumen.AppendLine("- Meta '" + meta.name + "': Tag Meta correcto.");
            }

            var col = meta.GetComponent<Collider2D>();
            if (col == null)
            {
                Undo.AddComponent<BoxCollider2D>(meta);
                resumen.AppendLine("- Meta: NO tenia collider. Se agrego un Box Collider 2D.");
            }
            else if (!col.enabled)
            {
                Undo.RecordObject(col, "Activar collider");
                col.enabled = true;
                resumen.AppendLine("- Meta: su collider estaba desactivado. Ya se activo.");
            }
            else
            {
                resumen.AppendLine("- Meta: collider correcto" + (col.isTrigger ? " (es Trigger; el script Jugador ya lo detecta)." : "."));
            }
        }

        // ---------- Picos y vacio ----------
        Camera cam = Camera.main;
        float centroX = cam != null ? cam.transform.position.x : 0f;
        float abajo = cam != null ? cam.transform.position.y - cam.orthographicSize : -5f;
        float ancho = cam != null ? cam.orthographicSize * 2f * 16f / 9f + 4f : 22f;

        Sprite cuadrado = CargarSprite("f4f8c47368758cf439b8bf4ae7483d3d", 7482667652216324306, "Assets/Sprites/Square.png");
        Material material = CargarMaterial("a97c105638bdf8b4a8650670310a4cd3");

        GameObject peligros = GameObject.Find("Peligros");
        if (peligros == null)
        {
            peligros = new GameObject("Peligros");
            Undo.RegisterCreatedObjectUndo(peligros, "Crear Peligros");
        }

        var picos = CrearOActualizar(peligros.transform, "Picos", cuadrado, material,
            new Vector3(centroX, abajo + 0.25f, 0f), new Vector3(ancho, 0.5f, 1f), new Color(0.9f, 0.15f, 0.15f));
        var vacio = CrearOActualizar(peligros.transform, "Vacio", cuadrado, material,
            new Vector3(centroX, abajo - 3f, 0f), new Vector3(ancho * 2f, 1f, 1f), Color.white);
        vacio.GetComponent<SpriteRenderer>().enabled = false; // invisible

        resumen.AppendLine("- Picos creados en Y = " + (abajo + 0.25f).ToString("0.00") + " (parte baja de la camara), Tag Peligro.");
        resumen.AppendLine("- Vacio (invisible) creado en Y = " + (abajo - 3f).ToString("0.00") + ", Tag Peligro.");

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        AssetDatabase.SaveAssets(); // guarda los Tags nuevos del proyecto
        Selection.activeGameObject = peligros;

        resumen.AppendLine("\nRecuerda guardar la escena con Ctrl+S.");
        Debug.Log(resumen.ToString());
        EditorUtility.DisplayDialog("Actividad 8", resumen.ToString(), "OK");
    }

    static GameObject BuscarMeta()
    {
        var todos = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var go in todos) if (go.CompareTag("Meta")) return go;
        foreach (var go in todos) if (go.name.ToLower() == "meta") return go;
        foreach (var go in todos)
        {
            string n = go.name.ToLower();
            if ((n.Contains("meta") || n.Contains("goal")) && !n.Contains("base")) return go;
        }
        return null;
    }

    static GameObject CrearOActualizar(Transform padre, string nombre, Sprite sprite, Material material, Vector3 posicion, Vector3 escala, Color color)
    {
        Transform existente = padre.Find(nombre);
        GameObject go;
        if (existente != null)
        {
            go = existente.gameObject;
            Undo.RecordObject(go.transform, "Actualizar " + nombre);
        }
        else
        {
            go = new GameObject(nombre);
            Undo.RegisterCreatedObjectUndo(go, "Crear " + nombre);
            go.transform.SetParent(padre, false);
        }

        go.transform.position = posicion;
        go.transform.localScale = escala;
        go.tag = "Peligro";

        var sr = go.GetComponent<SpriteRenderer>();
        if (sr == null) sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        if (material != null) sr.sharedMaterial = material;

        var col = go.GetComponent<BoxCollider2D>();
        if (col == null) col = go.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;
        col.isTrigger = false;

        return go;
    }

    static Sprite CargarSprite(string guid, long idLocal, string rutaAlternativa)
    {
        string ruta = AssetDatabase.GUIDToAssetPath(guid);
        if (string.IsNullOrEmpty(ruta)) ruta = rutaAlternativa;
        Sprite primero = null;
        foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(ruta))
        {
            if (obj is Sprite s)
            {
                if (primero == null) primero = s;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(s, out string _, out long id) && id == idLocal) return s;
            }
        }
        return primero;
    }

    static Material CargarMaterial(string guid)
    {
        string ruta = AssetDatabase.GUIDToAssetPath(guid);
        return string.IsNullOrEmpty(ruta) ? null : AssetDatabase.LoadAssetAtPath<Material>(ruta);
    }

    static void AgregarTag(string tag, StringBuilder resumen)
    {
        var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var tags = tagManager.FindProperty("tags");
        for (int i = 0; i < tags.arraySize; i++)
        {
            if (tags.GetArrayElementAtIndex(i).stringValue == tag) return;
        }
        tags.InsertArrayElementAtIndex(tags.arraySize);
        tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
        tagManager.ApplyModifiedPropertiesWithoutUndo();
        resumen.AppendLine("- Tag '" + tag + "' no existia en el proyecto. Ya se creo.");
    }
}
