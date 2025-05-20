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
    /// <summary>
    /// 던전 곂침 검사하는 오브젝트가 제거되는 시간 (DungeonCreator Class에서 시작할떄에 사용자가 입력한 값으로 되도록 할것임)
    /// </summary>
    public float inspectionDestroyTime = default;
    /// <summary>
    /// 던전 검사기 
    /// </summary>
    private WaitForSeconds inspectionDesTime = default;
    public WaitForSeconds InspectionDesTime
    {
        get
        {            
            if(inspectionDesTime == default || inspectionDesTime == null)
            {         
                inspectionDesTime = new WaitForSeconds(inspectionDestroyTime);
                return inspectionDesTime;
            }
            return inspectionDesTime;
        }

    }


    private void Awake()
    {
        isOverlap = false;
        isMakeTag = false;
        inspectionDestroyTime = default;

    }       // Awake()


}       // DungeonIspection
