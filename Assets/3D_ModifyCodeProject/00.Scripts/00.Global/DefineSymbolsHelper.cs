using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

#if UNITY_EDITOR
public class DefineSymbolsHelper : MonoBehaviour
{
    [Header("NowSettingSymbols")]
    public List<string> nowSettingSymbolsList = new List<string>();

    [Header("Setting")]
    [Tooltip("추가할 디파인심볼의 이름")]
    public string addSymbolName;
    [Tooltip("제거할 디파인 심볼의 이름")]
    public string removeSymbolName;

    [HideInInspector]
    public bool isSettingUp = false;


    /// <summary>
    /// 필드에 존재하는 AddSymbolName의 디파인심볼을 프로젝트 셋팅에 추가해주는함수
    /// </summary>
    public void AddDefineSymbol()
    {
        isSettingUp = true;
        // 현재 적용된 디파인 심볼을 Get
        string nowSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);

        if (CheckException(addSymbolName) == true)
        {       // 입력값이 비었는지 체크
            isSettingUp = false;
            return;
        }
        else { /*PASS*/ }


        StartAddSymbol(nowSymbols);
        ListUpdate();

        

    }       // AddDefineSymbol()

    public void RemoveDefineSymbol()
    {
        isSettingUp = true;
        // 현재 적용된 디파인 심볼을 Get
        string nowSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);

        if (CheckException(removeSymbolName) == true)
        {
            isSettingUp = false;
            return;
        }
        else { /*PASS*/ }

        if (nowSymbols.Contains(removeSymbolName))
        {   // 현재 디파인 심볼에 입력한 디파인심볼이 없을경우에만 진입            

            StartReMoveSymbol(nowSymbols);
            ListUpdate();
        }
        else
        {
            Debug.Log($"! 입력하신 디파인심볼이 현재 적용상태가 아닙니다. !\n" +
                $"! The defineSymbol you entered is not currently in application. !");
            isSettingUp = false;
        }


    }       // RemoveDefineSymbol()

    /// <summary>
    /// 질질적인 제거작업을 해주는 함수
    /// </summary>
    /// <param name="_nowSymbols">현재 적용된 디파인심볼 </param>
    private void StartReMoveSymbol(string _nowSymbols)
    {
        string reslutSymbols;
        if (_nowSymbols.Length == CalculationValue.ZERO)
        {   // 적용된 디파인 심볼이 존재하지 않는데 제거를 시도할 경우
            Debug.Log("! 현재 적용된 디파인 심볼이 존재하지 않습니다. !\n" +
                "! The currently applied DefineSymbol does not exist. !");
        }
        else if (_nowSymbols.Length > CalculationValue.ZERO)
        {   // 이미 적용된 심볼이 존재할 경우
            string[] nowSymbolArr = _nowSymbols.Split(';');            

            if (nowSymbolArr[0] == removeSymbolName)
            {   // 제거할 심볼이 맨 앞쪽에 존재한다면
                reslutSymbols = _nowSymbols.Replace(removeSymbolName, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, reslutSymbols);
            }
            else
            {
                reslutSymbols = _nowSymbols.Replace(";" + removeSymbolName, "");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, reslutSymbols);
            }
                        
        }
        else
        {   // 단 한개의 심볼만 존재할 경우
            reslutSymbols = "";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, reslutSymbols);

            Debug.Log("else 에서 팅기나?");
        }
    }       // StartReMoveSymbol()



    /// <summary>
    /// 실질적인 추가를 해주는 작업을 하는 함수
    /// </summary>
    /// <param name="_nowSymbols">현재 적용된 디파인심볼</param>
    private void StartAddSymbol(string _nowSymbols)
    {
        if (!_nowSymbols.Contains(addSymbolName))
        {   // If : 추가할 심볼이 이미 적용 되어있지 않을경우
            if (_nowSymbols.Length > CalculationValue.ZERO)
            {       // if : 추가하는 심볼 의외에 이미 적용된 심볼이 존재한다면
                string reslutSymbols = _nowSymbols + ";" + addSymbolName;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, reslutSymbols);
            }
            else
            {       // 현재 적용된 심볼이 존재하지 않다면
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, addSymbolName);
            }
        }
        else
        {   // else : 추가할 심볼이 이미 적용되어있는 상태라면
            Debug.Log($"! 입력한 값이 이미 적용되어 있습니다. !\n" +
                $"! The value you entered is already in effect. !");
        }
    }       // StartAddSymbol()

    /// <summary>
    /// 입력된 값이 비어있을경우의 예외처리
    /// </summary>
    /// <param name="_checkString">추가/제거 하려고 입력된 값</param>
    /// <returns>빈공간 = true , 빈공간이 아닐경우 = false</returns>
    private bool CheckException(string _checkString)
    {
        if (_checkString == " " || _checkString == "")
        {
            Debug.Log("! 입력한 값이 비어있는거 같습니다. !\n" +
                "The value you entered seems to be empty");
            return true;
        }
        return false;
    }       // CheckException()

    /// <summary>
    /// 현재 적용된 심볼이 담긴 리스트를 업데이트하는 함수 (사용시 명시적으로 매개변수에 true를 넣어주어야함)
    /// </summary>
    /// <param name="_isAdd"></param>
    /// <param name="_isRemove"></param>
    private void ListUpdate()
    {
        nowSettingSymbolsList.Clear();

        string nowSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
           EditorUserBuildSettings.selectedBuildTargetGroup);

        string[] nowSymbolsArr = nowSymbols.Split(";");

        nowSettingSymbolsList = nowSymbolsArr.ToList();

        isSettingUp = false;
    }       // ListUpdate()


}       // DefineSymbolsHelper ClassEnd

#endif
