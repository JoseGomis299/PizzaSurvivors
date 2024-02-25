using UnityEngine;

public class HideIfNotEqualAttribute : PropertyAttribute
{
    public string FieldName { get; private set; }
    public int[] Values { get; private set; }

    public HideIfNotEqualAttribute(string fieldName, int[] values)
    {
        FieldName = fieldName;
        Values = values;
    }
}