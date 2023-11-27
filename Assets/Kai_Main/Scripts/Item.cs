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
        bool m_active = false;

        private void Start()
        {
            StartCoroutine(snooze());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_active)
                return;

            if (m_toolPrefab != null)
                if (other.gameObject.TryGetComponent<ToolHolder>(out var holder))
                {
                    holder.Pickup(m_toolPrefab);
                    Destroy(gameObject);
                }
        }

        IEnumerator snooze()
        {
            yield return new WaitForSeconds(1f);
            m_active = true;
        }
    }
}