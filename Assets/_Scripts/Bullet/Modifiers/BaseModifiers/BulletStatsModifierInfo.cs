using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Modifiers/Stats Modifier", fileName = "Bullet Stats Modifier")]
public class BulletStatsModifierInfo : BulletModifierInfo
{
    [Header("Modifier Settings")]
    public float multiplier;
    
    public enum StatsModifierType
    {
        Size
    }
    
    [Header("Type")]
    public StatsModifierType type;
    
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        return type switch
        {
            StatsModifierType.Size => new BulletSizeModifier(target, maxLevel, remainsAfterHit, priority, multiplier),
            _ => null
        };
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BulletStatsModifierInfo))]
public class BulletStatsModifierInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, "maxLevel", "remainsAfterHit", "priority");
    }
}
#endif