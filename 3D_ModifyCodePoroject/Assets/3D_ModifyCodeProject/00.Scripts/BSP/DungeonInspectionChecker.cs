using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonInspectionChecker : MonoBehaviour
{       // 던전이 곂치는지 확인해줄 객체가 가질 컴포넌트

    // 싱글턴 패턴
    private static DungeonInspectionChecker instance;
    public static DungeonInspectionChecker Instance
    {
        get 
        {
            if(instance == null || instance == default)
            {
                GameObject obj = new GameObject("DungeonInspectionChecker");
                instance = obj.transform.AddComponent<DungeonInspectionChecker>();
            }
            return instance;
        }
    }

    /// <summary>
    /// 곂침판정 (곂치면 True)
    /// </summary>
    public bool isOverlap = default;
    /// <summary>
    /// 동적 태그생성 했다면 True
    /// </summary>
    public bool isMakeTag = default;
    /// <summary>
    /// 만들 태그의 이름
    /// </summary>
    public const string MAKETAGNAME = "DungeonInspection";

    private void Awake()
    {
        isOverlap = false;
        isMakeTag = false;
    }       // Awake()


}       // DungeonIspection
