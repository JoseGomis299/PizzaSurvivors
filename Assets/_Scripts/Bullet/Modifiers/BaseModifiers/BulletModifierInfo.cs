using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BulletModifierInfo : ScriptableObject
{
    [Header("Modifier Info")]
    public new string name;
    [SerializeField] private string[] descriptionsByLevel;
    
    [Header("Modifier Settings")]
    public int maxLevel;
    public int remainsAfterHit;
    public int priority;

    public abstract BulletModifier GetModifier(IEffectTarget target);

    public string GetDescription(int level)
    {
        if(level < 0) return null;
        return level < descriptionsByLevel.Length ? descriptionsByLevel[level] : descriptionsByLevel[^1];
    }
    
    public IEnumerable<string> GetDescriptions()
    {
        return descriptionsByLevel;
    }
}




