using UnityEditor;
using UnityEngine;

public abstract class BulletModifierInfo : Descriptor
{
    [Header("Modifier Settings")]
    public int maxLevel;
    public int priority;
}