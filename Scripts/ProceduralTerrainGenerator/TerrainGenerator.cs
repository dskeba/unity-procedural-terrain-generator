using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour
{
    public DrawMode drawMode;
    public Renderer textureRender;
    public GameObject mesh;
    public TerrainData terrainData;
    public bool runTerrainObjectSpawner = false;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private void Start()
    {
        GetComponents();
        ValidateTerrainData();
        Generate();
    }

    private void GetComponents()
    {
        meshFilter = mesh.GetComponent<MeshFilter>();
        meshRenderer = mesh.GetComponent<MeshRenderer>();
        meshCollider = mesh.GetComponent<MeshCollider>();
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
        LandMassGenerator.Generate(drawMode, terrainData.chunkSize, terrainData, textureRender, meshFilter, meshRenderer, meshCollider);

        if (runTerrainObjectSpawner)
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