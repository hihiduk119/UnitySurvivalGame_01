using UnityEngine;
using UnityEditor;

using WoosanStudio.ZombieShooter;

[CustomEditor(typeof(JsonLoad))]
public class JsonLoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        JsonLoad jsonLoad = (JsonLoad)target;
        if (GUILayout.Button("Load"))
        {
            jsonLoad.Load();
        }
    }
}
