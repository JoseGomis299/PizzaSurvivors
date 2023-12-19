using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private Stats stats;
    private Stats _stats;

    public Stats BaseStats => stats;
    public Stats Stats => _stats;
    
    private EffectManager _effectManager;

    private void Awake() {
        _stats = ScriptableObject.CreateInstance<Stats>();
        _stats.SetValues(stats);
        _effectManager = new EffectManager(this);
    }
    
    public void ApplyEffect(IEffect effect) {
        if(!gameObject.activeInHierarchy) return;
        
        _effectManager.ApplyEffect(effect as BaseEffect);
    }
    
    public void ReApplyEffects() {
        if(!gameObject.activeInHierarchy) return;

        _effectManager.ReApplyEffects();
    }

    private void OnDisable()
    {
        _effectManager.DeApplyAllEffects();
    }

    private void OnDestroy()
    {
        _effectManager.DeApplyAllEffects();
    }
}