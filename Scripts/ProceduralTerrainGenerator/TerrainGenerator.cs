using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour
{
    public DrawMode drawMode;
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

        ValidateTerrainData();

        if (drawMode == DrawMode.Mesh)
        {
            CreateMesh("TerrainMesh");
        } 
        else if (drawMode == DrawMode.NoiseMap)
        {
            CreateTextureRenderer("TerrainNoiseMap");
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            CreateTextureRenderer("TerrainColorMap");
        }

        Generate();
    }

    private void CreateMesh(string gameObjectName)
    {
        meshObject = new GameObject(gameObjectName);
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshCollider = meshObject.AddComponent<MeshCollider>();
    }

    private void CreateTextureRenderer(string gameObjectName)
    {
        GameObject go = new GameObject(gameObjectName);
        textureRenderer = go.AddComponent<MeshRenderer>();
        textureRenderer.material = new Material(Shader.Find("Unlit/Texture"));
    }

    private void ValidateTerrainData()
    {
        if (terrainData.useFlatShading &&
            terrainData.chunkSize > 105)
        {
            Debug.LogWarning("The provided TerrainData object has Flat Shading enabled and a Chunk Size of " + terrainData.chunkSize + ". Flat Shading only supports a maximum Chunk Size <= 105. Chunk Size will automatically be updated to 105.");
            terrainData.chunkSize = 105;
        }
    }

    public void Generate()
    {

        Debug.Log("[TerrainGenerator] Begin");
        LandMassGenerator.Generate(drawMode, terrainData.chunkSize, terrainData, textureRenderer, meshFilter, meshRenderer, meshCollider);

        if (runFloraGenerator && drawMode == DrawMode.Mesh)
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