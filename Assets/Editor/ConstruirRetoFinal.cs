using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Herramienta de apoyo: construye la escena del Reto Final con la configuracion correcta.
// Uso: menu  Herramientas > Construir Reto Final Equilibrio
// Puedes borrar este archivo cuando ya no lo necesites.
public static class ConstruirRetoFinal
{
    const string RutaEscena = "Assets/Scenes/Reto_Final_Equilibrio.unity";

    [MenuItem("Herramientas/Construir Reto Final Equilibrio")]
    public static void Construir()
    {
        // Pregunta si quieres guardar la escena abierta antes de continuar
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        AgregarTag("Suelo");

        var escena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Sprite cuadrado = CargarSprite("f4f8c47368758cf439b8bf4ae7483d3d", 7482667652216324306, "Assets/Sprites/Square.png");
        Sprite circulo = CargarSprite("ed38b21177e1a9740bdb214ffff46e42", -2413806693520163455, "Assets/Sprites/Circle.png");
        Material materialSprite = CargarMaterial("a97c105638bdf8b4a8650670310a4cd3");

        // ---------- Camara ----------
        var camara = new GameObject("Main Camera");
        camara.tag = "MainCamera";
        camara.transform.position = new Vector3(0f, 0f, -10f);
        var cam = camara.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.19f, 0.30f, 0.47f);
        camara.AddComponent<AudioListener>();

        // ---------- Nave ----------
        var nave = CrearSprite("Nave", cuadrado, materialSprite, new Vector3(0f, -1f, 0f), new Vector3(3f, 0.5f, 1f), new Color(0.85f, 0.85f, 0.85f));
        nave.AddComponent<BoxCollider2D>();
        var rbNave = nave.AddComponent<Rigidbody2D>();
        rbNave.bodyType = RigidbodyType2D.Dynamic;
        rbNave.gravityScale = 0f;                                  // no cae por si sola
        rbNave.linearDamping = 1f;                                 // menos inercia
        rbNave.constraints = RigidbodyConstraints2D.FreezeRotation; // no gira al recibir la pelota
        nave.AddComponent<CambioColor>();
        nave.AddComponent<NaveEquilibrio>();

        // ---------- Pelota ----------
        var pelota = CrearSprite("Pelota", circulo, materialSprite, new Vector3(0f, 0f, 0f), new Vector3(0.5f, 0.5f, 1f), new Color(1f, 0.85f, 0.3f));
        pelota.AddComponent<CircleCollider2D>();
        var rbPelota = pelota.AddComponent<Rigidbody2D>();
        rbPelota.bodyType = RigidbodyType2D.Dynamic;
        rbPelota.gravityScale = 1f;
        rbPelota.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // evita atravesar la nave
        rbPelota.interpolation = RigidbodyInterpolation2D.Interpolate;
        pelota.AddComponent<ControlPelota>();

        // ---------- Suelo ----------
        var suelo = CrearSprite("Suelo", cuadrado, materialSprite, new Vector3(0f, -4f, 0f), new Vector3(14f, 1f, 1f), new Color(0.35f, 0.25f, 0.2f));
        suelo.AddComponent<BoxCollider2D>(); // estatico: sin Rigidbody2D
        suelo.tag = "Suelo";

        EditorSceneManager.SaveScene(escena, RutaEscena);
        Selection.activeGameObject = nave;
        Debug.Log("Reto_Final_Equilibrio construido y guardado en " + RutaEscena);
    }

    static GameObject CrearSprite(string nombre, Sprite sprite, Material material, Vector3 posicion, Vector3 escala, Color color)
    {
        var go = new GameObject(nombre);
        go.transform.position = posicion;
        go.transform.localScale = escala;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        if (material != null) sr.sharedMaterial = material;
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
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(s, out string _, out long id) && id == idLocal)
                    return s;
            }
        }
        return primero;
    }

    static Material CargarMaterial(string guid)
    {
        string ruta = AssetDatabase.GUIDToAssetPath(guid);
        return string.IsNullOrEmpty(ruta) ? null : AssetDatabase.LoadAssetAtPath<Material>(ruta);
    }

    static void AgregarTag(string tag)
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
    }
}
