using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct BuffData 
{
    [SerializeField] private MonoScript type;
    
    public float multiplier;
    public IncrementType incrementType;
    
    public BaseEffect GetBuff(IEffectTarget target)
    {
        return Activator.CreateInstance(type.GetClass(), target, 1, 1, TimerType.Infinite, multiplier,  incrementType) as BaseEffect;
    }
}
