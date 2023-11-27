using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHolder : MonoBehaviour
{
    private Tool m_currentTool = null;

    public void Pickup(GameObject toolPrefab)
    {
        if (m_currentTool != null)
            m_currentTool.Drop();
        m_currentTool = null;

        var newTool = Instantiate(toolPrefab, transform);
        m_currentTool = newTool.GetComponent<Tool>();
    }
}
