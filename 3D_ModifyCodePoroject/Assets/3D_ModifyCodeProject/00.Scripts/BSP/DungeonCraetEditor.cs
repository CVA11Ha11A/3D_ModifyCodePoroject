using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonCreator))]
public class DungeonCraetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DungeonCreator dungeonCreator = (DungeonCreator)target;        
        if (GUILayout.Button("CreateNewDungeon"))
        {
            dungeonCreator.CreateDungeon();
        }
    }
}
#endif