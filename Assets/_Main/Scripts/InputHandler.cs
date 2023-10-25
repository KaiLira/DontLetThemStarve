using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Transform m_destination;

    private Vector3 m_panWorldStart;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            m_destination.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
