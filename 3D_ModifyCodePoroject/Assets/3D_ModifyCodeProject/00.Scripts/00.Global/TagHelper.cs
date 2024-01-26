using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
public class TagHelper : MonoBehaviour
{   // 던전의 사용할 태그추가를 도와주는 클래스
    // 제작 이유 : UnityEditorInternal.InternalEditorUtility.tags 관련은 빌드시 사용불가(에디터에서만 사용가능)
    //            그리고 에디터로 무언갈 많이 만들어보고 싶었음 2024.01.23

    public List<string> nowSettingTagList = new List<string>();

    [SerializeField]
    private string addTagName = "";
    [SerializeField]
    private string removeTagName = "";

    [HideInInspector]
    public bool isChanging = false;     // 테그 수정중인지 확인할 bool값

    public void AddTag()
    {
        isChanging = true;

        bool isNondefault = CheckDefault(isAdd: true);
        // 현재 선언되어있는 모든 태그를 가져옴
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
        foreach (string tag in tags)
        {
            if (tag == addTagName)
            {   // ! 이미 테그가 존재할 경우 !
                isChanging = false;
                Debug.Log("! 이미 같은 태그가 존재합니다. !\n! The same tag already exists. !");
                return;
            }
            else { /*PASS*/ }
        }
        UnityEditorInternal.InternalEditorUtility.AddTag(addTagName);
        isChanging = false;
        UpdateTagList();
        Debug.Log($"태그 생성완료, 생성된 태그 : {addTagName}\nTag creation completed, tag created : {addTagName}");

    }       // AddTag()

    public void RemoveTag()
    {
        isChanging = true;

        bool isNondefault = CheckDefault(isRemove: true);
        // 현재 선언되어있는 모든 태그를 가져옴
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

        foreach (string tag in tags)
        {
            if (tag == removeTagName)
            {   // 제거할 태그를 찾은경우
                UnityEditorInternal.InternalEditorUtility.RemoveTag(removeTagName);
                Debug.Log($"태그 제거완료, 제거된 태그 : {removeTagName}\nTag remove completed, tag removed : {removeTagName}");
                isChanging = false;
                UpdateTagList();
                return;
            }
            else { /*PASS*/ }
        }
        Debug.Log("! 제거할 태그를 찾지 못했습니다. !\n! No tags found to remove. !");
        isChanging = false;
    }       // RemoveTag()

    /// <summary>
    /// 실수로 빈값을 넣었는지 체크해줄 예외처리
    /// </summary>
    /// <param name="isAdd">현재 Add함수인지</param>
    /// <param name="isRemove">현재 Remove함수인지</param>
    /// <returns></returns>
    private bool CheckDefault(bool isAdd = false, bool isRemove = false)
    {
        if (isAdd == true)
        {
            if (addTagName == "" || addTagName == " " ||
                addTagName == default || addTagName == null)
            {
                Debug.Log("! 추가할 테그의 이름이 비어있습니다. !\n! The name of the tag you want to add is empty !");
                return true;
            }
            else { /*PASS*/ }
        }
        else if (isRemove == true)
        {

            if (removeTagName == "" || removeTagName == " " ||
            removeTagName == default || removeTagName == null)
            {
                Debug.Log("! 제거할 테그의 이름이 비어있습니다. !\n! The name of the tag you want to remove is empty !");
            }
        }
        else
        {
            Debug.Log("잘못된 예외 처리입니다.");
            return true;
        }
        return false;
    }       // CheckDefault()


    private void UpdateTagList()
    {
        nowSettingTagList = UnityEditorInternal.InternalEditorUtility.tags.ToList();
    }       // UpdateTagList()

}       // DungeonTagHelper Class
#endif
