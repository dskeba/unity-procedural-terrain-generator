using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrainSystem : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;

    public int seed = 0;

    private GameObject terrain;

    private System.Random random;

    private void Awake()
    {
        random = new System.Random(seed);
    }

    void Start()
    {
        StartCoroutine(GenerateAfterSeconds(10f));
    }

    private IEnumerator GenerateAfterSeconds(float seconds)
    {
        for (int i = 0; i < 1000; i++)
        {
            Object.Destroy(terrain);
            terrainGenerator.terrainData = RandomTerrainData();
            terrain = terrainGenerator.Generate();
            yield return new WaitForSeconds(seconds);
        }
    }

    private TerrainData RandomTerrainData()
    {
        TerrainData data = ScriptableObject.CreateInstance<TerrainData>();

        data.chunkSize = 105;

        data.useFlatShading = true;
        data.meshHeightMultiplier = random.Next(5, 15);
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0, 0);
        curve.AddKey(0.5f, (float)random.NextDouble());
        curve.AddKey(1, 1);
        data.meshHeightCurve = curve;

        data.noiseScale = random.Next(10, 20);
        data.octaves = random.Next(3, 6);
        data.persistance = (float)random.NextDouble() * (0.6f - 0.1f) + 0.1f;
        data.lacunarity = 2;
        data.seed = 0;
        data.offset = new Vector2(random.Next(0, 100), random.Next(0, 100));

        Debug.Log("noiseScale = " + data.noiseScale);
        Debug.Log("persistance = " + data.persistance);

        data.enableBlur = true;
        data.blurRadius = random.Next(2, 4);
        data.blurIterations = 1;

        int totalColorRegions = 10;
        data.colorRegions = new TerrainType[totalColorRegions];
        for (int i = 0; i < totalColorRegions; i++)
        {
            data.colorRegions[i] = new TerrainType();
            data.colorRegions[i].height = (float)((i + 1f) / totalColorRegions);
            data.colorRegions[i].color = new Color(
                (float)random.NextDouble() * (0.8f - 0.2f) + 0.2f,
                (float)random.NextDouble() * (0.8f - 0.2f) + 0.2f,
                (float)random.NextDouble() * (0.8f - 0.2f) + 0.2f);
        }

        data.floraSeed = random.Next(2000, 5000);
        data.floraRegions = new TerrainFloraType[4];

        // Grass
        data.floraRegions[0] = new TerrainFloraType();
        data.floraRegions[0].minHeight = 0f;
        data.floraRegions[0].maxHeight = 1f;
        data.floraRegions[0].spawnFrequency = 0.02f;
        data.floraRegions[0].prefabs = new GameObject[3];
        data.floraRegions[0].prefabs[0] = Resources.Load<GameObject>("Prefabs/Flora/FloraGrass1");
        data.floraRegions[0].prefabs[1] = Resources.Load<GameObject>("Prefabs/Flora/FloraGrass2");
        data.floraRegions[0].prefabs[2] = Resources.Load<GameObject>("Prefabs/Flora/FloraGrass3");
        data.floraRegions[0].randomizeScale = true;
        data.floraRegions[0].randomizeMaterialColor = true;

        // Bush
        data.floraRegions[1] = new TerrainFloraType();
        data.floraRegions[1].minHeight = 0f;
        data.floraRegions[1].maxHeight = 0.5f;
        data.floraRegions[1].spawnFrequency = 0.001f;
        data.floraRegions[1].prefabs = new GameObject[3];
        data.floraRegions[1].prefabs[0] = Resources.Load<GameObject>("Prefabs/Flora/FloraBush1");
        data.floraRegions[1].prefabs[1] = Resources.Load<GameObject>("Prefabs/Flora/FloraBush2");
        data.floraRegions[1].prefabs[2] = Resources.Load<GameObject>("Prefabs/Flora/FloraBush3");
        data.floraRegions[1].randomizeScale = true;
        data.floraRegions[1].randomizeMaterialColor = true;

        // Tree
        data.floraRegions[2] = new TerrainFloraType();
        data.floraRegions[2].minHeight = 0f;
        data.floraRegions[2].maxHeight = 0.5f;
        data.floraRegions[2].spawnFrequency = 0.001f;
        data.floraRegions[2].prefabs = new GameObject[3];
        data.floraRegions[2].prefabs[0] = Resources.Load<GameObject>("Prefabs/Flora/FloraTree1");
        data.floraRegions[2].prefabs[1] = Resources.Load<GameObject>("Prefabs/Flora/FloraTree2");
        data.floraRegions[2].prefabs[2] = Resources.Load<GameObject>("Prefabs/Flora/FloraTree3");
        data.floraRegions[2].randomizeScale = true;
        data.floraRegions[2].randomizeMaterialColor = true;

        // Rock
        data.floraRegions[3] = new TerrainFloraType();
        data.floraRegions[3].minHeight = 0f;
        data.floraRegions[3].maxHeight = 1f;
        data.floraRegions[3].spawnFrequency = 0.0005f;
        data.floraRegions[3].prefabs = new GameObject[3];
        data.floraRegions[3].prefabs[0] = Resources.Load<GameObject>("Prefabs/Flora/FloraRock1");
        data.floraRegions[3].prefabs[1] = Resources.Load<GameObject>("Prefabs/Flora/FloraRock2");
        data.floraRegions[3].prefabs[2] = Resources.Load<GameObject>("Prefabs/Flora/FloraRock3");
        data.floraRegions[3].randomizeScale = true;
        data.floraRegions[3].randomizeMaterialColor = true;

        return data;
    }
}
