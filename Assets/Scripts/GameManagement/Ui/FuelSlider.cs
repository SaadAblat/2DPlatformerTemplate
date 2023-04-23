using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FuelSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    public void SetFuel(float Fuel)
    {
        slider.value = Fuel;
    }
    public void SetMaxFuel(float Fuel)
    {
        slider.maxValue = Fuel;
        slider.value = Fuel;
    }
}
