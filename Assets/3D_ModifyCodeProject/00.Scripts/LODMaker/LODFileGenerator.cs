using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class LODFileGenerator : EditorWindow
{
    // 결과물 저장할떄에 사용하기위해 static으로 띄워둠
    public static string folderPatch = "Assets/01.SG_LOD";

    /// <summary>
    /// 제작시 저장해둘 폴더가 존재하지 않을 경우 저장하는 함수
    /// </summary>
    public static void LODForderCreate()
    {
        // 경로 (Assets 뒤에 오는경로 까지 체크하여 해당 폴더 경로가 존재하는지 체크)
        

        if (!AssetDatabase.IsValidFolder(folderPatch))
        {
            AssetDatabase.CreateFolder("Assets", "01.SG_LOD");
            //Debug.Log($"폴더 생성됨\n경로 : {folderPatch}");
        }
        else
        {
            /*PASS*/
            /*Debug.Log("이미 폴더가 존재합니다.");*/
        }

    }       // LODForderCreate()

    /// <summary>
    /// 매개변수로 받은 Object를 폴더에 저장하는 함수
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    /// <param name="saveData">최적화된 Mesh (Object타입이면 사용가능)</param>
    public static void LODFileSave<T>(T saveData) where T : Object
    {
        AssetDatabase.CreateAsset(saveData, "Assets/01.SG_LOD");
        //AssetDatabase.SaveAssets();   // 필요한가?
    }       // LODFileSave()
}       // ClassEnd
#endif