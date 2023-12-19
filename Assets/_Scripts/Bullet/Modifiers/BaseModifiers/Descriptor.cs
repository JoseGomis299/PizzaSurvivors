using System.Collections.Generic;
using UnityEngine;

public class Descriptor : ScriptableObject
{
    [Header("Info")]
    public new string name;
    [SerializeField] protected string[] descriptions;
    
    public string GetDescription(int level)
    {
        if(level < 0) return null;
        return level < descriptions.Length ? descriptions[level] : descriptions[^1];
    }
    
    public IEnumerable<string> GetDescriptions()
    {
        return descriptions;
    }
}