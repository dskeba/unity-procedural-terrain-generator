using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : ScriptableObject
{
	[Header("Terrain Generator")]
	[Tooltip("Flat Shading only supports chunk size < 105")]
	[Range(2, 255)]
	public int chunkSize;

	[Header("Mesh Generator")]
	[Tooltip("Flat Shading only supports chunk size < 105")]
	public bool useFlatShading;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

	[Header("Noise Generator")]
	public float noiseScale;
	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;
	public int seed;
	public Vector2 offset;

	[Header ("Texture")]
	public bool enableBlur;
	public int blurRadius;
	public int blurIterations;
	public TerrainType[] colorRegions;

	[Header("Flora")]
	public TerrainFloraType[] floraRegions;

	void OnValidate()
	{
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}
		if (octaves < 0)
		{
			octaves = 0;
		}
	}
}
