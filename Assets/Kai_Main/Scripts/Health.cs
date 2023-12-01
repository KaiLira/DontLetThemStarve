using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Kaicita
{
    public enum Substance
    {
        Flesh = 0,
        Wood = 1,
    }

    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int m_maxHealth;
        [SerializeField]
        private Substance m_substance;
        [SerializeField]
        private UnityEvent<float> m_onHealthChange;
        [SerializeField]
        private UnityEvent m_onHealthDeplete;

        private int m_currentHealth;

        public int MaxHealth { get { return m_maxHealth; } }
        public int CurrentHealth { get { return m_currentHealth; } }
        public float NormalizedHealth { get { return m_currentHealth / m_maxHealth; } }

        public static Dictionary<Substance, List<Health>> Damageables = new()
                {
                    { Substance.Flesh, new() },
                    { Substance.Wood, new() }
                };

        private void OnEnable()
        {
            m_currentHealth = m_maxHealth;
            Damageables[m_substance].Add(this); 
        }

        private void OnDisable()
        {
            Damageables[m_substance].Remove(this);
        }

        public void Damage(int amount)
        {
            m_currentHealth -= amount;
            if (m_currentHealth <= 0)
                m_onHealthDeplete?.Invoke();
            else
                m_onHealthChange?.Invoke(NormalizedHealth);
        }
    }
}