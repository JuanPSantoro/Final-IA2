using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsUI : MonoBehaviour
{
    private Dictionary<string, GameObject> _tools;
    private GameObject _currentTool;

    void Start()
    {
        _tools = new Dictionary<string, GameObject>();
        _tools.Add(Items.AXE.ToString(), transform.Find("Axe").gameObject);
        _tools.Add(Items.HAMMER.ToString(), transform.Find("Hammer").gameObject);
        _tools.Add(Items.SCYTHE.ToString(), transform.Find("Scythe").gameObject);
        _tools.Add(Items.WEAPON.ToString(), transform.Find("Sword").gameObject);
        EventManager.instance.AddEventListener(EventType.TOOL_CHANGE, onToolChange);
    }

    private void onToolChange(object[] parameters)
    {
        SetTool((string)parameters[0]);
    }

    public void SetTool(string newTool)
    {
        if (_tools.ContainsKey(newTool))
        {
            if (_currentTool != null)
                _currentTool.SetActive(false);
            _currentTool = _tools[newTool];
            _currentTool.SetActive(true);
        }
    }

}
