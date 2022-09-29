using UnityEngine;
using System.Collections;
using System.Linq;

public static class FloraGenerator
{

	public static void GenerateFlora(MeshFilter meshFilter, TerrainData terrainData)
	{
        Vector3[] vertices = meshFilter.sharedMesh.vertices;

        if (vertices.Length < 1) return;
        if (terrainData.floraRegions.Length < 1) return;

        int totalObjectSpawned = 0;

        System.Random random = new System.Random(terrainData.seed);

        float maxY = vertices.OrderByDescending(vertex => vertex.y).First().y;

        Debug.Log("[FloraGenerator] Total Vertices in Mesh: " + vertices.Length);

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
                        Object.Instantiate(terrainData.floraRegions[j].prefab, vertices[i], Quaternion.identity);
                        totalObjectSpawned++;
                    }
                }
            }
        }

        Debug.Log("[FloraGenerator] Total Objects Spawned = " + totalObjectSpawned);

        Debug.Log("[FloraGenerator] Tallest vertex = " + maxY);
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
