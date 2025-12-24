using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthSlider3D;
    public Slider healthSlider2D;

    public void Start3DSlider(float maxValue)
    {
        healthSlider3D.maxValue = maxValue;
        healthSlider3D.value = maxValue;
    }

    // Update is called once per frame
    public void Update3DSlider(float Value)
    {
        healthSlider3D.value = Value;
    }

    public void Update2DSlider(float maxValue, float Value)
    {
        if (gameObject.CompareTag("Player"))
        {
            healthSlider2D.maxValue = maxValue;
            healthSlider2D.value = Value;
        }
    }
}
