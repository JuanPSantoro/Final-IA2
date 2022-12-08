using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsUI : HiddeableUI
{
    [SerializeField]
    private Text _logText = default;

    override protected void Start()
    {
        Clear();
        EventManager.instance.AddEventListener(EventType.FSM_FINISH, OnFSMFinished);
        EventManager.instance.AddEventListener(EventType.RE_PLAN, OnReplan);
        base.Start();
    }

    private void OnReplan(object[] parameters)
    {
        Clear();
    }

    private void OnFSMFinished(object[] parameters)
    {
        Clear();
        HideUI();
    }

    public void LogText(string text)
    {
        _logText.text += text;
    }

    public void Clear()
    {
        _logText.text = "";
    }
}
