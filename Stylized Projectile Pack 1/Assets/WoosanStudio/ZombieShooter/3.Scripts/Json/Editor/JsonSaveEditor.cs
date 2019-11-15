using UnityEngine;
using UnityEditor;

using WoosanStudio.ZombieShooter;

[CustomEditor(typeof(JsonSave))]
public class JsonSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        JsonSave jsonSave = (JsonSave)target;
        if (GUILayout.Button("Add Item"))
        {
            jsonSave.AddWeapon();
        }

        if (GUILayout.Button("Save"))
        {
            jsonSave.Save();
        }
    }
}
