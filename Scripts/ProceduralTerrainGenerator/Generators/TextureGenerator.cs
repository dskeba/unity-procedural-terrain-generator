﻿using UnityEngine;
using System.Collections;

public static class TextureGenerator {

	public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height, TerrainData terrainData) {
		Texture2D texture = new Texture2D(width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colorMap);
		texture.Apply();
		if (terrainData.enableBlur)
		{
			texture = Blur.FastBlur(texture, terrainData.blurRadius, terrainData.blurIterations);
		}
		return texture;
	}

	public static Texture2D TextureFromHeightMap(float[,] heightMap, TerrainData terrainData) {
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp(Color.black, Color.white, heightMap [x, y]);
			}
		}

		return TextureFromColorMap(colourMap, width, height, terrainData);
	}

}