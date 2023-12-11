using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Stats Effect", fileName = "Stats Effect", order = 0)]
public class EffectInfo : ScriptableObject
{
    [Header("Effect Type")]
    
    public MonoScript type;
    
    [Header("Effect Settings")]
    public float multiplier;
    public int maxStacks;
    public float duration;
    
    [Space(10)]
    public IncrementType incrementType;
    public TimerType timerType;
    
    public BaseEffect GetEffect(IEffectTarget target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, duration, maxStacks, timerType, multiplier,  incrementType) as BaseEffect;
    }
}