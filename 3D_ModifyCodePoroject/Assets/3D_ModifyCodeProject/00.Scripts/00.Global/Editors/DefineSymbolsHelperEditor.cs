using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefineSymbolsHelper))]
public class DefineSymbolsHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        float inspectorWidth;   // 인스팩터의 넓이

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("AddSymbol", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f),
            GUILayout.Height(30)))
        {
            DefineSymbolsHelper helper = (DefineSymbolsHelper)target;
            helper.AddDefineSymbol();
        }
        else { /*PASS*/ }

        if (GUILayout.Button("RemoveSymbol", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f)
            , GUILayout.Height(30)))
        {
            DefineSymbolsHelper helper = (DefineSymbolsHelper)target;
            helper.RemoveDefineSymbol();
        }
        else { /*PASS*/ }
        GUILayout.EndHorizontal();

    }

}       // DefineSymbolsHelperEditor ClassEnd
