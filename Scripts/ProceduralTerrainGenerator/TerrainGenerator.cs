using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour
{
    public DrawMode drawMode;
    public int mapChunkSize;
    public Renderer textureRender;
    public GameObject mesh;
    public TerrainData terrainData;
    public bool runTerrainObjectSpawner = false;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private void Start()
    {
        meshFilter = mesh.GetComponent<MeshFilter>();
        meshRenderer = mesh.GetComponent<MeshRenderer>();
        meshCollider = mesh.GetComponent<MeshCollider>();
        Generate();
    }

    public void GenerateLandMass()
    {
        LandMassGenerator.Generate(drawMode, mapChunkSize, terrainData, textureRender, meshFilter, meshRenderer, meshCollider);
    }

    public void Generate()
    {
        Debug.Log("[TerrainGenerator] Begin");
        GenerateLandMass();

        if (runTerrainObjectSpawner)
        {
            Debug.Log("[TerrainGenerator] Spawn Flora");
            SpawnFlora();
        }
        Debug.Log("[TerrainGenerator] Complete");
    }

    public void SpawnFlora()
    {
        Vector3[] vertices = meshFilter.sharedMesh.vertices;

        if (vertices.Length < 1) return;
        if (terrainData.floraRegions.Length < 1) return;

        int totalObjectSpawned = 0;

        System.Random random = new System.Random(terrainData.seed);

        float maxY = vertices.OrderByDescending(vertex => vertex.y).First().y;

        Debug.Log("[TerrainGenerator] Total Vertices in Mesh: " + vertices.Length);

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = 0; j < terrainData.floraRegions.Length; j++)
            {
                float vertexHeightScale = vertices[i].y / maxY;
                if (vertexHeightScale > terrainData.floraRegions[j].minHeight &&
                    vertexHeightScale <= terrainData.floraRegions[j].maxHeight)
                {
                    float randomValue = (float)random.NextDouble();

                    if (terrainData.floraRegions[j].spawnFrequency > randomValue)
                    {
                        Instantiate(terrainData.floraRegions[j].prefab, vertices[i], Quaternion.identity);
                        totalObjectSpawned++;
                    }
                }
            }
        }

        Debug.Log("[TerrainGenerator] Total Objects Spawned = " + totalObjectSpawned);

        Debug.Log("[TerrainGenerator] Tallest vertex = " + maxY);
    }
}

[System.Serializable]
public struct TerrainFloraType
{
    public string name;
    [Range(0, 1)]
    public float minHeight;
    [Range(0, 1)]
    public float maxHeight;
    [Range(0, 1)]
    public float spawnFrequency;
    public GameObject prefab;
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