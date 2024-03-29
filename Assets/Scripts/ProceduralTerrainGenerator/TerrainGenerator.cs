using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public TerrainData terrainData;
    public bool runFloraGenerator = true;
    public bool generateOnStart = true;

    private GameObject meshObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private Renderer textureRenderer;

    private void Start()
    {
        if (!generateOnStart) return;
        Generate();
    }

    public GameObject Generate()
    {
        ValidateTerrainData();
        CreateMesh("TerrainMesh");
        GenerateTerrain();
        return meshObject;
    }

    private void CreateMesh(string gameObjectName)
    {
        // Create a mesh filter, renderer, and collider for drawing terrain
        meshObject = new GameObject(gameObjectName);
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshCollider = meshObject.AddComponent<MeshCollider>();
    }

    private void ValidateTerrainData()
    {
        // Flat shading only supports chunkSize <= 105
        if (terrainData.useFlatShading &&
            terrainData.chunkSize > 105)
        {
            Debug.LogWarning("The provided TerrainData object has Flat Shading enabled and a Chunk Size of " + terrainData.chunkSize + ". Flat Shading only supports a maximum Chunk Size <= 105. Chunk Size will automatically be updated to 105.");
            terrainData.chunkSize = 105;
        }
    }

    private void GenerateTerrain()
    {

        Debug.Log("[TerrainGenerator] Begin");
        LandMassGenerator.Generate(terrainData.chunkSize, terrainData, textureRenderer, meshFilter, meshRenderer, meshCollider);

        // Only run flora generator if its enabled
        if (runFloraGenerator)
        {
            Debug.Log("[TerrainGenerator] Spawn Flora");
            FloraGenerator.GenerateFlora(meshFilter, terrainData);
        }
        Debug.Log("[TerrainGenerator] Complete");
    }
}

/*[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TerrainGenerator terrainGenerator = (TerrainGenerator)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Preview Land Mass"))
        {
            terrainGenerator.GenerateLandMass();
        }
    }
}*/