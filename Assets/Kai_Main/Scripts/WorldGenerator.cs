using ProceduralNoiseProject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Kaicita
{
    [RequireComponent(typeof(Grid))]
    public class WorldGenerator : MonoBehaviour
    {
        [Header("Noise settings")]
        [SerializeField]
        private bool m_generateOnStart = true;
        [SerializeField]
        private bool m_randomizeSeed = true;
        [SerializeField]
        private int m_seed = 0;
        [SerializeField]
        private float m_frequency = 0f;
        [SerializeField]
        private float m_jitter = 0f;

        [Space]
        [Header("Map size")]
        [SerializeField, Min(0)]
        private int m_width;
        [SerializeField, Min(0)]
        private int m_depth;
        [SerializeField, Min(0)]
        private int m_safeRadius;
        [SerializeField]
        private AnimationCurve m_safeCurve;
        [SerializeField, Min(0)]
        private int m_fadeOffRadius;
        [SerializeField]
        private AnimationCurve m_fadeOffCurve;

        [Space]
        [Header("Tiles")]
        [SerializeField]
        List<Threshold> m_thresholds;

        private Noise m_mainNoise;
        private Noise m_secondaryNoise;
        private Vector2 m_center;

        private Tile GenerateTile(int x, int y, List<Threshold> thresholds)
        {
            float value = m_mainNoise.Sample2D(x, y);
            value += m_secondaryNoise.Sample2D(x, y) - m_secondaryNoise.Frequency / 2f;

            float fromCenter = Vector2.Distance(new (x, y), m_center);
            
            if (fromCenter < m_safeRadius)
            {
                // 0 is at the center, 1 is at the limit of the safe radius
                float normalized = 1f - Mathf.Clamp01(fromCenter / m_safeRadius);
                float sample = m_safeCurve.Evaluate(normalized);
                value = Mathf.Clamp01(value + sample);
            }

            if (fromCenter > m_fadeOffRadius)
            {
                // 0 is at the fade of radius and 1 is at the "edge" of the generated world
                float normalized = Mathf.Clamp01(
                    (fromCenter - m_fadeOffRadius) /
                    ((m_width + m_depth) / 4f)
                    );

                float sample = m_fadeOffCurve.Evaluate(normalized);
                value = Mathf.Clamp01(value - sample);
            }

            foreach (Threshold threshold in thresholds)
                if (value <= threshold.max)
                    return threshold.tile;

            return thresholds.Last().tile;
        }

        public void GenerateMap(int sizeX, int sizeY)
        {
            GameObject map = new("Map");
            Tilemap tilemap = map.AddComponent<Tilemap>();
            TilemapRenderer renderer = map.AddComponent<TilemapRenderer>();
            renderer.sortingLayerName = "Terrain";
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
            m_secondaryNoise = new WorleyNoise(
                m_seed >> 8,
                m_frequency * 2f,
                m_jitter * 1.5f, 1f/3f
                );

            m_center = new (m_width/2, m_depth/2);
            GenerateMap(m_width, m_depth);
        }

        private void Start()
        {
            if (m_generateOnStart)
            {
                GenerateWorld();
                StartCoroutine(ScanNextFrame());
            }
        }


        IEnumerator ScanNextFrame()
        {
            yield return 0;
            AstarPath.active.Scan();
        }
    }


    [Serializable]
    struct Threshold
    {
        public float max;
        public Tile tile;
    }
}