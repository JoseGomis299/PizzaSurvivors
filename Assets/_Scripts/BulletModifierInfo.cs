using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/BulletModifier", fileName = "BulletModifier")]
public class BulletModifierInfo : ScriptableObject
{
    [Header("Modifier Info")]
    public new string name;
    [SerializeField] private string[] descriptionsByLevel;

    [Header("Modifier Type")]
    
    public MonoScript type;
    
    [Header("Modifier Settings")]
    public int maxLevel;

    public BulletModifier GetModifier(IEffectTarget target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, maxLevel) as BulletModifier;
    }
    
    public string GetDescription(int level)
    {
        if(level < 0) return null;
        return level < descriptionsByLevel.Length ? descriptionsByLevel[level] : descriptionsByLevel[^1];
    }
}