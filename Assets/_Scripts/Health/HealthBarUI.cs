using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthComponent))]
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();

        _healthComponent.OnMaxHealthUpdate += UpdateMaxValue;
        _healthComponent.OnHealthUpdate += UpdateSlider;
    }

    private void OnDestroy()
    {
        _healthComponent.OnHealthUpdate -= UpdateSlider;
        _healthComponent.OnMaxHealthUpdate -= UpdateMaxValue;
    }

    private void UpdateSlider(float health)
    {
        healthBarSlider.value = health;
    }
    
    private void UpdateMaxValue(float maxHealth)
    {
        healthBarSlider.maxValue = maxHealth;
    }
}
