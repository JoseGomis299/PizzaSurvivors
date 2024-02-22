using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Attack))]
public class AttackDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        SerializedProperty shapeType = property.FindPropertyRelative("ShapeType");
        SerializedProperty orientation = property.FindPropertyRelative("Orientation");
        SerializedProperty size = property.FindPropertyRelative("Size");
        SerializedProperty matchRange = property.FindPropertyRelative("MatchRange");
        SerializedProperty relativePosition = property.FindPropertyRelative("RelativePosition");
        
        position.height = EditorGUIUtility.singleLineHeight;
        
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("damageMultiplier"));
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("element"));
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("knockback"));
        position.y += EditorGUIUtility.singleLineHeight;
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, shapeType);
        position.y += EditorGUIUtility.singleLineHeight;

        if (!matchRange.boolValue)
        {
            EditorGUI.PropertyField(position, relativePosition);
            position.y += EditorGUIUtility.singleLineHeight;
        }

        switch (shapeType.enumValueIndex)
        {
            case (int)ShapeType.Capsule:
                if (matchRange.boolValue)
                {
                    size.vector2Value = new Vector2(EditorGUI.FloatField(position, "Size X", size.vector2Value.x), size.vector2Value.y);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                else
                {
                    EditorGUI.PropertyField(position, size);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                EditorGUI.PropertyField(position, orientation);
                position.y += EditorGUIUtility.singleLineHeight;
                break;
            case (int)ShapeType.Circle:
                if (!matchRange.boolValue)
                {
                    size.vector2Value = new Vector2(EditorGUI.FloatField(position, "Radius", size.vector2Value.x), size.vector2Value.y);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                break;
            case (int)ShapeType.Line:
                if (!matchRange.boolValue)
                {
                    size.vector2Value = new Vector2(EditorGUI.FloatField(position, "Length", size.vector2Value.x), size.vector2Value.y);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                break;
            default:
                if (matchRange.boolValue)
                {
                    size.vector2Value = new Vector2(EditorGUI.FloatField(position, "Size X", size.vector2Value.x), size.vector2Value.y);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                else
                {
                    EditorGUI.PropertyField(position, size);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
                break;
        }

        if (shapeType.enumValueIndex != (int)ShapeType.Line || !matchRange.boolValue)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative("Angle"));
            position.y += EditorGUIUtility.singleLineHeight;
        }

        EditorGUI.PropertyField(position, matchRange);

        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty shapeType = property.FindPropertyRelative("ShapeType");
        SerializedProperty matchRange = property.FindPropertyRelative("MatchRange");

        float height = EditorGUIUtility.singleLineHeight * 10;
        if (shapeType.enumValueIndex == (int)ShapeType.Capsule) height += EditorGUIUtility.singleLineHeight;
        if(matchRange.boolValue) height -=  EditorGUIUtility.singleLineHeight;
        if(!(shapeType.enumValueIndex == (int)ShapeType.Capsule || shapeType.enumValueIndex == (int)ShapeType.Rectangle) && matchRange.boolValue) height -= EditorGUIUtility.singleLineHeight;
        if (shapeType.enumValueIndex == (int)ShapeType.Line && matchRange.boolValue) height -= EditorGUIUtility.singleLineHeight;
        return height;
    }
}