using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthComponent))]
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    private HealthComponent healthComponent;

    private void Start()
    {
        healthComponent = GetComponent<HealthComponent>();

        healthComponent.onHealthUpdate += UpdateSlider;
        healthBarSlider.maxValue = healthComponent.Health;
        healthBarSlider.value = healthComponent.Health;
    }

    private void OnDestroy()
    {
        healthComponent.onHealthUpdate -= UpdateSlider;
    }

    private void UpdateSlider(float health)
    {
        healthBarSlider.value = health;
    }
}
