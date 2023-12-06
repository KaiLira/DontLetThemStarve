using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShow : MonoBehaviour
{
    int m_currentSlide;

    private void setSlide(int slide)
    {
        transform.GetChild(m_currentSlide).gameObject.SetActive(false);
        m_currentSlide = slide;
        transform.GetChild(m_currentSlide).gameObject.SetActive(true);

    }

    public void NextSlide()
    {
        setSlide(Math.Clamp(m_currentSlide + 1, 0, transform.childCount - 1));
    }

    public void PreviousSlide()
    {
        setSlide(Math.Clamp(m_currentSlide - 1, 0, transform.childCount - 1));
    }

    void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        transform.GetChild(0).gameObject.SetActive(true);
    }
}
