using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Transform m_destination;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            m_destination.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
