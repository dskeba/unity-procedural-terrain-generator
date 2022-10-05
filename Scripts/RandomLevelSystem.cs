using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelSystem : MonoBehaviour
{
    public int seed;
    public TerrainGenerator TerrainGenerator;
    public GameObject obelisk;

    private FloraEraser floraEraser;
    private TerrainData terrainData;

    void Start()
    {
        floraEraser = CreateFloraEraser();
        GenerateLevel(seed);
        floraEraser.DestroyOverlappingFlora();
        SpawnObelisk();
    }

    private FloraEraser CreateFloraEraser()
    {
        GameObject go = new GameObject("FloraEraser");
        go.transform.position = Vector3.zero;
        go.transform.localScale = new Vector3(20, 100, 20);
        FloraEraser floraEraser = go.AddComponent<FloraEraser>();
        floraEraser.Mode = FloraEraserMode.RunManual;
        floraEraser.LayerMaskToDestroy = 1 << LayerMask.NameToLayer("Flora");
        return floraEraser;
    }

    private void GenerateLevel(int level)
    {
        terrainData = RandomTerrainData.Generate(seed + level);
        TerrainGenerator.terrainData = terrainData;
        TerrainGenerator.Generate();
        floraEraser.DestroyOverlappingFlora();
    }

    private void SpawnObelisk()
    {
        Object.Instantiate(obelisk);
    }
}
