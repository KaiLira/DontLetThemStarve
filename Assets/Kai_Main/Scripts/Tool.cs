using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField]
    GameObject m_itemPrefab;

    public void Drop()
    {
        Instantiate(m_itemPrefab, transform.position + Vector3.right, Quaternion.identity);
        Destroy(gameObject);
    }
}
