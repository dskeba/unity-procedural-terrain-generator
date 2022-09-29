using UnityEngine;
using System.Collections;
using UnityEditor;

public enum DrawMode { NoiseMap, ColourMap, Mesh };

public static class LandMassGenerator{

    public static void Generate(DrawMode drawMode, int mapChunkSize, TerrainData terrainData, Renderer textureRender, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider) {

		float[,] noiseMap = NoiseGenerator.GenerateNoiseMap (mapChunkSize, mapChunkSize, terrainData.seed, terrainData.noiseScale, terrainData.octaves, terrainData.persistance, terrainData.lacunarity, terrainData.offset);

		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < terrainData.colorRegions.Length; i++) {
					if (currentHeight <= terrainData.colorRegions[i].height) {
						colourMap [y * mapChunkSize + x] = terrainData.colorRegions[i].colour;
						break;
					}
				}
			}
		}

		if (drawMode == DrawMode.NoiseMap) {
			DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap, terrainData), textureRender);
		} else if (drawMode == DrawMode.ColourMap) {
			DrawTexture(TextureGenerator.TextureFromColorMap(colourMap, mapChunkSize, mapChunkSize, terrainData), textureRender);
		} else if (drawMode == DrawMode.Mesh) {
			DrawMesh(MeshGenerator.GenerateTerrainMesh (noiseMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, terrainData.useFlatShading), TextureGenerator.TextureFromColorMap(colourMap, mapChunkSize, mapChunkSize, terrainData), meshFilter, meshCollider, meshRenderer);
		}
	}

	public static void DrawTexture(Texture2D texture, Renderer textureRender)
	{
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
	}

	public static void DrawMesh(MeshData meshData, Texture2D texture, MeshFilter meshFilter, MeshCollider meshCollider, MeshRenderer meshRenderer)
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