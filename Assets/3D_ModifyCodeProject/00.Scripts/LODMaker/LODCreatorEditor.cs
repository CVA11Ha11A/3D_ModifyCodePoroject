using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(LODCreator))]
public class LODCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LODCreator lodCreator = (LODCreator)target;
        if (GUILayout.Button("LOD_Create"))
        {
            lodCreator.CreateLOD();
        }
    }
}       // ClassEnd
#endif