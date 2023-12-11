using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bullet Modifier", fileName = "Bullet Modifier")]
public class BulletModifierInfo : ScriptableObject
{
    [Header("Modifier Info")]
    public new string name;
    [SerializeField] private string[] descriptionsByLevel;

    [Header("Modifier Type")]
    public MonoScript type;
    
    [Header("Modifier Settings")]
    public int maxLevel;
    public int remainsAfterHit;
    public int priority;

    public virtual BulletModifier GetModifier(IEffectTarget target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, maxLevel, remainsAfterHit, priority) as BulletModifier;
    }
    
    public string GetDescription(int level)
    {
        if(level < 0) return null;
        return level < descriptionsByLevel.Length ? descriptionsByLevel[level] : descriptionsByLevel[^1];
    }
}