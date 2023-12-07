using System;
using System.Collections.Generic;

public class EffectManager
{
    private readonly Dictionary<Type, BaseEffect> _buffs = new Dictionary<Type, BaseEffect>();
    
    private readonly StatsManager _statsManager;
    
    public EffectManager(StatsManager statsManager) {
        _statsManager = statsManager;
    }
    
    public void ApplyEffect(BaseEffect effect)
    {
        if(!_buffs.ContainsKey(effect.GetType()))
            _buffs.Add(effect.GetType(), effect);
        
        _buffs[effect.GetType()].Apply();
    }

    public void ReApplyEffects()
    {
        _statsManager.Stats.SetValues(_statsManager.BaseStats);

        foreach (var myBuff in _buffs)
        {
            myBuff.Value.ReApply();
        }
    }
}