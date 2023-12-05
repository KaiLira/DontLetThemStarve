using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashSprite : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Flash(float seconds)
    {
        m_spriteRenderer.enabled = !m_spriteRenderer.enabled;
        StartCoroutine(resetAfter(seconds));
    }

    IEnumerator resetAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_spriteRenderer.enabled = !m_spriteRenderer.enabled;
    }
}
