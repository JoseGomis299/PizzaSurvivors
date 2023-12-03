using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Effect", fileName = "Effect", order = 0)]
public class Effect : ScriptableObject
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
    
    public BaseEffect GetEffect(StatsManager target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, duration, maxStacks, timerType, multiplier,  incrementType) as BaseEffect;
    }
}
