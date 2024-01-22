using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonInspection : MonoBehaviour
{       // 던전 검사기(객체)가 가질 컴포넌트
    //private BoxCollider boxCollider = default;
    private void Awake()
    {
        SettingTag();
        //boxCollider = this.transform.AddComponent<BoxCollider>();
    }

    private void OnCollisionStay(Collision collision)
    {
        // 만든 객체(곂치는지 검하는 객체)가 아닐때엔 곂쳐도 return을 시키도록조건 제작
        // 그리고 곂침판정이 이미 true라면 return
        // 위 2조건이 모두 아니라면 (곂침검사기 끼리 곂침 && 곂침 판정이 false)곂침 판정 true
        if(!collision.transform.CompareTag(DungeonInspectionChecker.MAKETAGNAME))
        {
            return;
        }
        else { /*PASS*/ }
        if(DungeonInspectionChecker.Instance.isOverlap == true)
        {
            return;
            // Pass
        }
        else
        {
            DungeonInspectionChecker.Instance.isOverlap = true;
        }
    }       // OnCollisionStay()

    private void SettingTag()
    {
        if (DungeonInspectionChecker.Instance.isMakeTag == false)
        {
            DungeonInspectionChecker.Instance.isMakeTag = true;
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

            foreach(string tag in tags)
            {
                if(tag == DungeonInspectionChecker.MAKETAGNAME)
                {
                    return;
                }
                else { /*PASS*/ }
            }
            UnityEditorInternal.InternalEditorUtility.AddTag(DungeonInspectionChecker.MAKETAGNAME);
            this.transform.tag = DungeonInspectionChecker.MAKETAGNAME;

        }
        else 
        {
            this.transform.tag = DungeonInspectionChecker.MAKETAGNAME;
        }
    }       // SettingTag()

}       // DungeonInspection Class
