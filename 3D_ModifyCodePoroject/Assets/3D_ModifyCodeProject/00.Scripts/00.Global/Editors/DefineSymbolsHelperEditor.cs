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
        if(GUILayout.Button("TESTMETHOD"))
        {
            DefineSymbolsHelper helper = (DefineSymbolsHelper)target;
            helper.TestMethod();
        }
    }

}       // DefineSymbolsHelperEditor ClassEnd
