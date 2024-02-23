using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager
{
    private readonly Dictionary<Type, BaseEffect> _effects = new Dictionary<Type, BaseEffect>();
    private HashSet<IncrementalEffect> _statBuffs = new HashSet<IncrementalEffect>();

    private readonly StatsManager _statsManager;
    
    public EffectManager(StatsManager statsManager) {
        _statsManager = statsManager;
    }
    
    public void ApplyEffect(BaseEffect effect)
    {
        if (effect is IncrementalEffect statBuff && effect.MaxStacks == 1)
        {
            _statBuffs.Add(statBuff);
            statBuff.Apply();
            statBuff.OnDeApply += () => _statBuffs.Remove(statBuff);
            return;
        }

        if(!_effects.ContainsKey(effect.GetType()))
            _effects.Add(effect.GetType(), effect);
        
        _effects[effect.GetType()].Apply();
    }
    
    public void DeApplyAllEffects()
    {
        foreach (var effect in _effects)
            effect.Value.DeApply();
        
        foreach (var buff in _statBuffs)
            buff.DeApply();
    }

    public void ReApplyEffects()
    {
        _statsManager.Stats.SetValues(_statsManager.BaseStats);

        foreach (var effect in _effects)
        {
            effect.Value.ReApply();
        }

        _statBuffs = _statBuffs.OrderBy(x => x.IncrementType).ToHashSet();
        foreach (var buff in _statBuffs)
        {
            buff.ReApply();
        }
    }
}