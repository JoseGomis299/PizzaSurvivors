using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HideIfNotEqualAttribute))]
public class HideIfNotEqualDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HideIfNotEqualAttribute hideIfNotEqualAttribute = (HideIfNotEqualAttribute)attribute;

        // Get the parent object of the property
        var parent = property.serializedObject.targetObject;

        // Use reflection to get the field value from the parent object
        var fieldInfo = parent.GetType().GetField(hideIfNotEqualAttribute.FieldName);
        if (fieldInfo != null)
        {
            var fieldValue = (int)fieldInfo.GetValue(parent);

            // Compare the field value with the value from the attribute
            bool shouldHide = true;
            foreach (var value in hideIfNotEqualAttribute.Values)
            {
                if (fieldValue == value)
                {
                    shouldHide = false;
                    break;
                }
            }
            if (!shouldHide)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}