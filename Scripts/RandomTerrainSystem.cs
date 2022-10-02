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
        StartCoroutine(GenerateAfterSeconds(0.1f));
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

        data.chunkSize = 105; // random.Next(55, 105);

        data.useFlatShading = true; // (random.NextDouble() >= 0.5);
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
        data.blurIterations = 1; // random.Next(1, 2);

        data.colorRegions = new TerrainType[10];
        for (int i = 0; i < 10; i++)
        {
            data.colorRegions[i] = new TerrainType();
            data.colorRegions[i].height = (float)((i + 1f) / 10f);
            data.colorRegions[i].color = new Color(
                (float)random.NextDouble() * (0.8f - 0.2f) + 0.2f,
                (float)random.NextDouble() * (0.8f - 0.2f) + 0.2f,
                (float)random.NextDouble() * (0.8f - 0.2f) + 0.2f);
        }

        return data;
    }
}
