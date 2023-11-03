using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kaicita
{
    [RequireComponent(typeof(Collider2D))]
    public class OrderListener : MonoBehaviour
    {
        [SerializeField]
        private Transform m_destination;
        public bool m_selected = false;
        private Collider2D m_collider;

        private void Start()
        {
            m_collider = GetComponent<Collider2D>();
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_selected)
                {
                    m_destination.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    m_selected = false;
                }
                else
                {
                    if (m_collider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                        m_selected = true;
                }
            }
        }
    }
}