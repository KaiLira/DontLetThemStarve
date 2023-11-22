using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kaicita
{
    [RequireComponent(typeof(Collider2D))]
    public class Item : MonoBehaviour
    {
        [SerializeField]
        GameObject m_toolPrefab;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (m_toolPrefab != null)
            {
                if (other.gameObject.TryGetComponent<ToolHolder>(out var holder))
                {
                    Instantiate(m_toolPrefab, holder.transform);
                    Destroy(gameObject);
                }
            }
        }
    }
}