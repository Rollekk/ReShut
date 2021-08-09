using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(AbsorberController))]
public class AbsorberEditor : Editor
{
    private SerializedProperty isSpecific;
    private SerializedProperty neededGunType;
    private SerializedProperty isActive;
    private SerializedProperty initialMaterialColor;

    private void OnEnable()
    {
        isSpecific = serializedObject.FindProperty("isSpecific");
        neededGunType = serializedObject.FindProperty("neededGunType");
        isActive = serializedObject.FindProperty("isActive");

        initialMaterialColor = serializedObject.FindProperty("initialMaterialColor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(isSpecific, new GUIContent("Is specific"));

        if(isSpecific.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(neededGunType, new GUIContent("needed Gun Type"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(isActive, new GUIContent("Is activated"));

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(initialMaterialColor, new GUIContent("Initial color"));
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
