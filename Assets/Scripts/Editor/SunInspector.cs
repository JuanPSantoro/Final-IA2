using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Sun))]

public class SunInspector : Editor
{
    private Sun _sun;

    private void OnEnable()
    {
        _sun = (Sun)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Sun"))
        {
            _sun.UpdateSun();
        }
    }
}
