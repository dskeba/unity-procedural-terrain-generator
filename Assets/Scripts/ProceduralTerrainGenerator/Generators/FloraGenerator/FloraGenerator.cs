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
            float vertexHeightScale = vertices[i].y / maxY;

            // Loop over all flora regions defined in the terrain data
            for (int j = 0; j < terrainData.floraRegions.Length; j++)
            {

                // Only attempt to add flora to vertices where height/y value
                // is within flora region threshhold
                if (vertexHeightScale > terrainData.floraRegions[j].minHeight &&
                    vertexHeightScale <= terrainData.floraRegions[j].maxHeight)
                {
                    // Random chance to add flora
                    float randomValue = (float)random.NextDouble();
                    if (terrainData.floraRegions[j].spawnFrequency > randomValue)
                    {
                        // Choose random prefab to use for flora region
                        int prefabIndex = random.Next(0, terrainData.floraRegions[j].prefabs.Length);

                        // Create the new flora object
                        GameObject go = Object.Instantiate(terrainData.floraRegions[j].prefabs[prefabIndex], vertices[i], Quaternion.Euler(new Vector3(0f, random.Next(0, 360))));

                        // Make flora object child of terrain
                        go.transform.parent = meshFilter.transform;

                        // Check if randomize scale is on
                        if (terrainData.floraRegions[j].randomizeScale)
                        {
                            go.transform.localScale = new Vector3(random.Next(1, 3), random.Next(1, 3), random.Next(1, 3));
                        }

                        // Check if randomize material color is on
                        if (terrainData.floraRegions[j].randomizeMaterialColor)
                        {
                            // Modify color of all materials on all renderers
                            // Passing in flora region index as seed ensures
                            // all regions have different colors
                            ChangeColorOfAllChildren(go, terrainData.floraSeed + j);
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
    public GameObject[] prefabs;
    public bool randomizeScale;
    public bool randomizeMaterialColor;
}
