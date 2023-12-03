using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Effect))]
public class EffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Effect effect = (Effect)target;
        
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(iterator, true);
        EditorGUI.EndDisabledGroup();
        
        // If timerType is Infinite, disable the 'duration' field
        while (iterator.NextVisible(false))
        {
            if (iterator.name == "duration" && effect.timerType == TimerType.Infinite) 
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(iterator, true);
                EditorGUI.EndDisabledGroup();
            }
            else EditorGUILayout.PropertyField(iterator, true);
        }

            // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}