using UnityEngine;
using UnityEditor;

public class PlacementIndicatorGenerator
{
    [MenuItem("Tools/Create Placement Indicator Prefab")]
    public static void CreateIndicator()
    {
        // Create root GameObject
        GameObject indicator = new GameObject("PlacementIndicator");

        // Add Cylinder
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.SetParent(indicator.transform);
        cylinder.transform.localPosition = Vector3.zero;
        cylinder.transform.localRotation = Quaternion.identity;
        cylinder.transform.localScale = new Vector3(0.3f, 0.01f, 0.3f);

        // Create transparent green material
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(0f, 1f, 0f, 0.5f); // RGBA
        mat.SetFloat("_Mode", 3); // Transparent mode
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        // Enable emission
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", new Color(0.3f, 1f, 0.3f));

        // Apply material to mesh renderer
        Renderer renderer = cylinder.GetComponent<Renderer>();
        renderer.sharedMaterial = mat;

        // Create folder if not exist
        string folderPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        // Save material
        string matPath = "Assets/Prefabs/PlacementIndicatorMaterial.mat";
        AssetDatabase.CreateAsset(mat, matPath);

        // Save prefab
        string prefabPath = "Assets/Prefabs/PlacementIndicator.prefab";
        PrefabUtility.SaveAsPrefabAsset(indicator, prefabPath);

        // Cleanup
        GameObject.DestroyImmediate(indicator);

        Debug.Log("✔ PlacementIndicator.prefab has been created and saved to Assets/Prefabs");
    }
}

