using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bullet Stats Modifier", fileName = "Bullet Stats Modifier")]
public class BulletStatsModifierInfo : BulletModifierInfo
{
    [Header("Modifier Settings")]
    public float multiplier;
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, maxLevel, remainsAfterHit, priority, multiplier) as BulletStatsModifier;
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