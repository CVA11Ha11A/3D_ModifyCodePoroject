using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DefineSymbolsHelper : MonoBehaviour
{
    [Tooltip("추가할 디파인심볼의 이름")]
    public string addSymbolName;
    [Tooltip("제거할 디파인 심볼의 이름")]
    public string removeSymbolName;


    public string test;
    public void TestMethod()
    {
        test = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);

        Debug.Log($"현재 적용된 Symbol : {test}");


        // 세미 클론 기준으로 현재 적용된 Define을 나누면 될거같음
        string cutting = "TEST;TOMANY";
    }       // TestMethod()

    /// <summary>
    /// 필드에 존재하는 AddSymbolName의 디파인심볼을 프로젝트 셋팅에 추가해주는함수
    /// </summary>
    public void AddDefineSymbol()
    {
        // 현재 적용된 디파인 심볼을 Get
        string nowSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);

        bool checkException = CheckException(addSymbolName);

        if(checkException == true)
        {
            return;
        }
        else { /*PASS*/ }
        
        if(!nowSymbols.Contains(addSymbolName))
        {
            string reslutSymbols = nowSymbols + ";" + addSymbolName;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, reslutSymbols);
        }
        else
        {
            Debug.Log($"! 입력한 값이 이미 적용되어 있습니다. !\n! The value you entered is already in effect. !");
        }
    }       // AddDefineSymbol()

    private bool CheckException(string _checkString)
    {
        if(_checkString == " " || _checkString == "")
        {
            return true;
        }
        return false;
    }
}       // DefineSymbolsHelper ClassEnd
