using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : ScriptableObject
{
    public bool useFlatShading;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

	public float noiseScale;

	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public TerrainType[] colorRegions;

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
