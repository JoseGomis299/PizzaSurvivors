using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager
{
    private readonly Dictionary<Type, BaseEffect> _effects = new Dictionary<Type, BaseEffect>();
    private List<IncrementalEffect> _statBuffs = new List<IncrementalEffect>();

    private readonly StatsManager _statsManager;
    
    public EffectManager(StatsManager statsManager) {
        _statsManager = statsManager;
    }
    
    public void ApplyEffect(BaseEffect effect)
    {
        if (effect is IncrementalEffect statBuff && effect.MaxStacks == 1)
        {
            IncrementalEffect existingBuff = _statBuffs.FirstOrDefault(x => x.IsEqual(statBuff));
            if (existingBuff == null || !existingBuff.IncreaseValue(statBuff.Increment))
            {
                _statBuffs.Add(statBuff);
                statBuff.OnDeApply += () => _statBuffs.Remove(statBuff);
                statBuff.Apply();
            }
            ReApplyEffects();
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

        _statBuffs = _statBuffs.OrderBy(x => x.IncrementType).ToList();
        foreach (var buff in _statBuffs)
        {
            buff.ReApply();
        }
    }
}