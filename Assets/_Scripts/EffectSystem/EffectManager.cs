using System;
using System.Collections.Generic;

public class EffectManager
{
    private readonly Dictionary<Type, BaseEffect> _effects = new Dictionary<Type, BaseEffect>();
    private readonly HashSet<StatsManagerEffect> _statBuffs = new HashSet<StatsManagerEffect>();

    private readonly StatsManager _statsManager;
    
    public EffectManager(StatsManager statsManager) {
        _statsManager = statsManager;
    }
    
    public void ApplyEffect(BaseEffect effect)
    {
        if (effect is StatsManagerEffect statBuff && effect.MaxStacks == 1)
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

    public void ReApplyEffects()
    {
        _statsManager.Stats.SetValues(_statsManager.BaseStats);

        foreach (var effect in _effects)
        {
            effect.Value.ReApply();
        }

        foreach (var buff in _statBuffs)
        {
            buff.ReApply();
        }
    }
}