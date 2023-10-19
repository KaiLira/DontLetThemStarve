using ProceduralNoiseProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    List<Threshold> m_thresholds;

    [SerializeField]
    private bool m_randomizeSeed = true;
    [SerializeField]
    private int m_seed = 0;
    [SerializeField]
    private float m_frequency = 0f;
    [SerializeField]
    private float m_jitter = 0f;

    private Noise m_mainNoise;
    private Noise m_secondaryNoise;

    private Tile GenerateTile(int x, int y, List<Threshold> thresholds)
    {
        float value = m_mainNoise.Sample2D(x, y);
        value += m_secondaryNoise.Sample2D(x, y) - m_secondaryNoise.Frequency / 2f;

        foreach (Threshold threshold in thresholds)
        {
            if (value <= threshold.max)
                return threshold.tile;
        }

        return thresholds.Last().tile;
    }

    public void GenerateMap(int sizeX, int sizeY)
    {
        GameObject map = new("Map");
        Tilemap tilemap = map.AddComponent<Tilemap>();
        map.AddComponent<TilemapRenderer>();
        map.AddComponent<TilemapCollider2D>();
        map.transform.SetParent(transform, false);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Tile tile = GenerateTile(x, y, m_thresholds);
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    [ContextMenu("Generate World")]
    private void GenerateWorld()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        if (m_randomizeSeed || m_seed == 0)
            m_seed = DateTime.Now.GetHashCode();

        m_mainNoise = new WorleyNoise(m_seed, m_frequency, m_jitter);
        m_secondaryNoise = new WorleyNoise(m_seed >> 8, m_frequency * 2f, m_jitter * 1.5f, 1f / 3f);

        GenerateMap(100, 100);
    }
}

[Serializable]
struct Threshold
{
    public float max;
    public Tile tile;
}