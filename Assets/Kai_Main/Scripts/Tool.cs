using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kaicita
{
    public class Tool : MonoBehaviour
    {
        [SerializeField]
        GameObject m_itemPrefab;
        [SerializeField]
        float m_range;
        [SerializeField]
        float m_cooldown;
        float m_timer;
        [SerializeField]
        int m_damage;
        [SerializeField]
        Substance m_targetSubstance;

        public void Drop()
        {
            Instantiate(m_itemPrefab, transform.position + Vector3.right, Quaternion.identity);
            Destroy(gameObject);
        }

        private void Update()
        {
            if (m_timer <= m_cooldown)
            {
                m_timer += Time.deltaTime;
                return;
            }

            m_timer = 0f;

            foreach (var hittable in Health.Damageables[m_targetSubstance])
                if (
                    hittable.gameObject != transform.parent.gameObject &&
                    Vector3.Distance(transform.position, hittable.transform.position) <= m_range
                    )
                {
                    hittable.Damage(m_damage);
                    return;
                }
        }
    }
}