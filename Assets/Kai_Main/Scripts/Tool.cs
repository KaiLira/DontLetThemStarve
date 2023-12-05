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
        float m_hitTimer;
        [SerializeField]
        int m_damage;
        [SerializeField]
        Substance m_targetSubstance;

        Allegiance m_allegiance = Allegiance.None;

        float m_animTimer;
        bool m_inPlace = true;

        public void Drop()
        {
            Instantiate(m_itemPrefab, transform.position + Vector3.right, Quaternion.identity);
            Destroy(gameObject);
        }

        private void Awake()
        {
            if (transform.parent.TryGetComponent<Health>(out var parentHealth))
            {
                m_allegiance = parentHealth.Allegiance;
            }
        }

        private void Update()
        {
            if (!m_inPlace)
            {
                m_animTimer += Time.deltaTime;
                if (m_animTimer >= m_cooldown / 5)
                {
                    m_animTimer = 0;
                    m_inPlace = true;
                    transform.rotation = Quaternion.identity;
                }
            }

            if (m_hitTimer <= m_cooldown)
            {
                m_hitTimer += Time.deltaTime;
                return;
            }

            m_hitTimer = 0f;

            foreach (var hittable in Health.Damageables[m_targetSubstance])
            {
                if (m_allegiance != Allegiance.None && m_allegiance == hittable.Allegiance)
                    continue;

                if (
                    hittable.gameObject != transform.parent.gameObject &&
                    Vector2.Distance(transform.position, hittable.transform.position) <= m_range
                    )
                {
                    hittable.Damage(m_damage);
                    m_inPlace = false;
                    float angle = Vector2.SignedAngle(
                        transform.position,
                        hittable.transform.position
                        ) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                    return;
                }
            }
                
        }
    }
}