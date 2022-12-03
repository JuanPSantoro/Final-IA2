using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]

public class WaypointInspector : Editor
{
    private Waypoint _wp;

    private void OnEnable()
    {
        _wp = (Waypoint)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Waypoint"))
        {
            _wp.CreateAndLink();
        }
    }
}
