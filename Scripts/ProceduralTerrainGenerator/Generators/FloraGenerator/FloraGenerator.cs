using UnityEngine;
using System.Linq;

public static class FloraGenerator
{

	public static void GenerateFlora(MeshFilter meshFilter, TerrainData terrainData)
	{
        Vector3[] vertices = meshFilter.sharedMesh.vertices;

        // Validate we have vertices and flora regions otherwise escape
        if (vertices == null || vertices.Length < 1) return;
        if (terrainData.floraRegions == null || terrainData.floraRegions.Length < 1) return;

        int totalObjectSpawned = 0;

        // Deterministic random from terrain data seed
        System.Random random = new System.Random(terrainData.seed);

        // Get the max vertex height
        float maxY = vertices.OrderByDescending(vertex => vertex.y).First().y;

        Debug.Log("[FloraGenerator] Total Vertices in Mesh: " + vertices.Length);

        // Loop over all vertices in the terrain mesh
        for (int i = 0; i < vertices.Length; i++)
        {
            // Loop over all flora regions defined in the terrain data
            for (int j = 0; j < terrainData.floraRegions.Length; j++)
            {
                float vertexHeightScale = vertices[i].y / maxY;

                // Only attempt to add flora to vertices where height/y value
                // is within flora region threshhold
                if (vertexHeightScale > terrainData.floraRegions[j].minHeight &&
                    vertexHeightScale <= terrainData.floraRegions[j].maxHeight)
                {
                    // Random chance to add flora
                    float randomValue = (float)random.NextDouble();

                    if (terrainData.floraRegions[j].spawnFrequency > randomValue)
                    {
                        // Create the new flora object
                        GameObject go = Object.Instantiate(terrainData.floraRegions[j].prefab, vertices[i], Quaternion.identity);

                        // Position flora at current vertex
                        go.transform.parent = meshFilter.transform;

                        // Check if randomize material color is on for object
                        if (terrainData.floraRegions[j].randomizeMaterialColor)
                        {
                            // Modify color of all materials on all renderers
                            // Passing in flora region index as seed ensures
                            // all regions have different colors
                            ChangeColorOfAllChildren(go, j);
                        }

                        totalObjectSpawned++;
                    }
                }
            }
        }

        Debug.Log("[FloraGenerator] Total Objects Spawned = " + totalObjectSpawned);

        Debug.Log("[FloraGenerator] Tallest vertex = " + maxY);
    }

    private static void ChangeColorOfAllChildren(GameObject parent, int seed)
    {
        System.Random random = new System.Random(seed);
        Renderer[] renderers = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.color = new Color(
                    (float)random.NextDouble() * (0.9f - 0.1f) + 0.1f,
                    (float)random.NextDouble() * (0.9f - 0.1f) + 0.1f,
                    (float)random.NextDouble() * (0.9f - 0.1f) + 0.1f);
            }
        }
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
    public bool randomizeMaterialColor;
}
