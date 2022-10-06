using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelSystem : MonoBehaviour
{
    public int seed;
    public TerrainGenerator TerrainGenerator;
    public GameObject obelisk;
    public GameObject player;

    private FloraEraser floraEraser;
    private TerrainData terrainData;
    private GameObject terrain;

    void Start()
    {
        // Generate level based on seed
        GenerateLevel(seed);

        // Create FloraEraser
        floraEraser = CreateFloraEraser();

        // Move Eraser to center of level and remove 10x10 area
        // for spawning the obelisk
        floraEraser.transform.position = Vector3.zero;
        floraEraser.transform.localScale = new Vector3(10, 100, 10);
        floraEraser.DestroyOverlappingFlora();
        SpawnObelisk();

        // Move Eraser to corner of level and remove 10x10 area
        // for spawning the player
        float playerSpawnSize = 10;
        float playerSpawnX = (terrainData.chunkSize / 2) - (playerSpawnSize / 2);
        float playerSpawnZ = (terrainData.chunkSize / 2) - (playerSpawnSize / 2);
        floraEraser.transform.position = new Vector3(playerSpawnX, 0, playerSpawnZ);
        floraEraser.transform.localScale = new Vector3(playerSpawnSize, 100, playerSpawnSize);
        floraEraser.DestroyOverlappingFlora();
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(playerSpawnX, 9999f, playerSpawnZ), Vector3.down);
        Physics.Raycast(ray, out hit);
        SpawnPlayer(hit.point + new Vector3(0f, 1f, 0f));
    }

    private FloraEraser CreateFloraEraser()
    {
        GameObject go = new GameObject("FloraEraser");
        FloraEraser floraEraser = go.AddComponent<FloraEraser>();
        floraEraser.Mode = FloraEraserMode.RunManual;
        floraEraser.LayerMaskToDestroy = 1 << LayerMask.NameToLayer("Flora");
        return floraEraser;
    }

    private void GenerateLevel(int level)
    {
        terrainData = RandomTerrainData.Generate(seed + level);
        TerrainGenerator.terrainData = terrainData;
        terrain = TerrainGenerator.Generate();
    }

    private void SpawnObelisk()
    {
        Object.Instantiate(obelisk);
    }

    private void SpawnPlayer(Vector3 position)
    {
        Object.Instantiate(player, position, Quaternion.identity);
    }
}
