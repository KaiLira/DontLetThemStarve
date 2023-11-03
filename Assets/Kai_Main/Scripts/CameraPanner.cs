using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kaicita
{
    public class CameraPanner : MonoBehaviour
    {
        private Vector3 m_panWorldStart;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                m_panWorldStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            else if (Input.GetMouseButton(1))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var diff = m_panWorldStart - mouseWorldPos;
                Camera.main.transform.position += diff;
            }
        }
    }
}