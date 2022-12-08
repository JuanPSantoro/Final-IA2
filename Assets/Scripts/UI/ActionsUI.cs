using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsUI : HiddeableUI
{
    [SerializeField]
    private Text _logText;

    protected void Start()
    {
        Clear();
        base.Start();
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
