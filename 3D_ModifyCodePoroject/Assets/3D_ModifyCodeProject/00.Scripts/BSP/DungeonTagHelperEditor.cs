using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonTagHelper))]
public class DungeonTagHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        float inspectorWidth;
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("AddTag", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f),
            GUILayout.Height(30)))
        {
            DungeonTagHelper dungeonTagHelper = (DungeonTagHelper)target;
            dungeonTagHelper.AddTag();
        }
        else { /*PASS*/ }

        if (GUILayout.Button("RemoveTag", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f)
            , GUILayout.Height(30)))
        {
            DungeonTagHelper dungeonTagHelper = (DungeonTagHelper)target;
            dungeonTagHelper.RemoveTag();
        }
        else { /*PASS*/ }
        GUILayout.EndHorizontal();

    }
}
#endif
