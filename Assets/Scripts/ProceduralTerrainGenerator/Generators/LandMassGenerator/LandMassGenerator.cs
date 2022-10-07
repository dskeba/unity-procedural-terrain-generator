using UnityEngine;
using System.Collections;
using UnityEditor;

public enum DrawMode { NoiseMap, ColorMap, Mesh };

public static class LandMassGenerator{

    public static void Generate(DrawMode drawMode, int mapChunkSize, TerrainData terrainData, Renderer textureRender, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider) {

		// Generate noise map using perlin noise
		float[,] noiseMap = NoiseGenerator.GenerateNoiseMap (mapChunkSize, mapChunkSize, terrainData.seed, terrainData.noiseScale, terrainData.octaves, terrainData.persistance, terrainData.lacunarity, terrainData.offset);

		// Create a color map based on noise map and color regions
		// from the terrain data
		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < terrainData.colorRegions.Length; i++) {
					if (currentHeight <= terrainData.colorRegions[i].height) {
						colorMap [y * mapChunkSize + x] = terrainData.colorRegions[i].color;
						break;
					}
				}
			}
		}

		// Determine what to draw based upon draw mode
		if (drawMode == DrawMode.NoiseMap) {
			DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap, terrainData), textureRender);
		} else if (drawMode == DrawMode.ColorMap) {
			DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize, terrainData), textureRender);
		} else if (drawMode == DrawMode.Mesh) {
			DrawMesh(MeshGenerator.GenerateTerrainMesh (noiseMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, terrainData.useFlatShading), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize, terrainData), meshFilter, meshCollider, meshRenderer);
		}
	}

	public static void DrawTexture(Texture2D texture, Renderer textureRender)
	{
		//textureRender.sharedMaterial.mainTexture = texture;
		textureRender.material.mainTexture = texture;
		textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
	}

	public static void DrawMesh(MeshData meshData, Texture2D texture, MeshFilter meshFilter, MeshCollider meshCollider, MeshRenderer meshRenderer)
	{
		Mesh mesh = meshData.CreateMesh();
		meshFilter.sharedMesh = mesh;
		meshCollider.sharedMesh = mesh;
		//meshRenderer.sharedMaterial.mainTexture = texture;
		meshRenderer.material.mainTexture = texture;
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	[Range(0, 1)]
	public float height;
	public Color color;
}