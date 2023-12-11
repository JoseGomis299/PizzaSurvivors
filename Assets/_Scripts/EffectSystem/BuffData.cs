using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct BuffData 
{
    [SerializeField] private MonoScript type;
    
    public float multiplier;
    public IncrementType incrementType;
    
    public BaseEffect GetEffect(IEffectTarget target)
    {
        return Activator.CreateInstance(type.GetClass(), target, 1, 9999, TimerType.Infinite, multiplier,  incrementType) as BaseEffect;
    }
}
