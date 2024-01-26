using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DefineSymbolsHelper))]
public class DefineSymbolsHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Debug.Log(PlayerSettings.GetScriptingDefineSymbolsForGroup(
        //    EditorUserBuildSettings.selectedBuildTargetGroup));

        DefineSymbolsHelper helper = (DefineSymbolsHelper)target;
        float inspectorWidth;   // 인스팩터의 넓이

        if(helper.nowSettingSymbolsList.Count == CalculationValue.ZERO && helper.isSettingUp == false)
        {
            string nowSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);

            string[] nowSymbolsArr = nowSymbols.Split(";");

            helper.nowSettingSymbolsList = nowSymbolsArr.ToList();            

        }
        else { /*PASS*/ }
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("AddSymbol", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f),
            GUILayout.Height(30)))
        {            
            helper.AddDefineSymbol();
        }
        else { /*PASS*/ }

        if (GUILayout.Button("RemoveSymbol", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f)
            , GUILayout.Height(30)))
        {            
            helper.RemoveDefineSymbol();
        }
        else { /*PASS*/ }

        GUILayout.EndHorizontal();

    }       // OnInspectorGUI()



}       // DefineSymbolsHelperEditor ClassEnd
#endif
