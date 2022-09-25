using UnityEngine;
using System.Collections;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

	public int mapChunkSize;

	public TerrainData terrainData;

	public Renderer textureRender; 
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public MeshCollider meshCollider;

    private void Start()
    {
		GenerateMap();
    }

    public void GenerateMap() {
		float[,] noiseMap = NoiseGenerator.GenerateNoiseMap (mapChunkSize, mapChunkSize, terrainData.seed, terrainData.noiseScale, terrainData.octaves, terrainData.persistance, terrainData.lacunarity, terrainData.offset);

		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < terrainData.regions.Length; i++) {
					if (currentHeight <= terrainData.regions[i].height) {
						colourMap [y * mapChunkSize + x] = terrainData.regions[i].colour;
						break;
					}
				}
			}
		}

		if (drawMode == DrawMode.NoiseMap) {
			DrawTexture(TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			DrawTexture(TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh) {
			DrawMesh(MeshGenerator.GenerateTerrainMesh (noiseMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, terrainData.useFlatShading), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		}
	}

	public void DrawTexture(Texture2D texture)
	{
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture)
	{
		Mesh mesh = meshData.CreateMesh();
		meshFilter.sharedMesh = mesh;
		meshCollider.sharedMesh = mesh;
		meshRenderer.sharedMaterial.mainTexture = texture;
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{

	public override void OnInspectorGUI()
	{
		TerrainGenerator terrainGenerator = (TerrainGenerator)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			terrainGenerator.GenerateMap();
		}
	}
}