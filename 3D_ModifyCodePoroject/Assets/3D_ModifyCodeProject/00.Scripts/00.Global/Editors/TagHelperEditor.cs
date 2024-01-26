using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(TagHelper))]
public class TagHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        float inspectorWidth;
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        TagHelper tagHelper = (TagHelper)target;

        if(tagHelper.isChanging == false && tagHelper.nowSettingTagList.Count == CalculationValue.ZERO)
        {
            tagHelper.nowSettingTagList = UnityEditorInternal.InternalEditorUtility.tags.ToList();
        }
        else { /*PASS*/ }

        if (GUILayout.Button("AddTag", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f),
            GUILayout.Height(30)))
        {
            tagHelper.AddTag();
        }
        else { /*PASS*/ }

        if (GUILayout.Button("RemoveTag", GUILayout.Width(inspectorWidth = EditorGUIUtility.currentViewWidth * 0.5f)
            , GUILayout.Height(30)))
        {        
            tagHelper.RemoveTag();
        }
        else { /*PASS*/ }
        GUILayout.EndHorizontal();

    }       // OnInspectorGUI()
}       // ClassEnd
#endif
