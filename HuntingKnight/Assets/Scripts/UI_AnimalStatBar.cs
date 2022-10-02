using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AnimalStatBar : MonoBehaviour
{
    public Slider slider;
    public string nameOfTheParent;

    public void Init()
    {
        slider = GetComponentInParent<Slider>();
        nameOfTheParent = transform.parent.name;
    }

    public void SetMax(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetCurrent(float health)
    {
        slider.value = health;
    }
}
