using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToolContainer : MonoBehaviour
{
    private Dictionary<string, Tool> _tools = new Dictionary<string, Tool>();

    private void Start()
    { 
        var allTools = GetComponentsInChildren<Tool>().ToList();
        foreach (var currentTool in allTools)
        {
            _tools.Add(currentTool.tool.ToString(), currentTool);
            currentTool.SetTool(transform);
        }
    }

    public void PickupTool(string tool, Transform newParent, Vector3 position)
    {
        Tool toolToPickUp;
        if (_tools.TryGetValue(tool, out toolToPickUp))
        {
            toolToPickUp.GrabTool(newParent, position);
        }
    }

    public void DropTool(string tool)
    {
        Tool toolToDrop;

        if (_tools.TryGetValue(tool, out toolToDrop))
        {
            toolToDrop.DropTool();
        }
    }
}
