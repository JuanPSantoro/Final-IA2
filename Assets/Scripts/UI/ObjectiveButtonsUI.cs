using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveButtonsUI : MonoBehaviour
{
    [SerializeField]
    private float _xHide;
    [SerializeField]
    private float _xShow;

    void Start()
    {
        EventManager.instance.AddEventListener(EventType.FSM_FINISH, OnFSMFinished);
        _xShow = transform.localPosition.x;
        _xHide += _xShow;
    }

    private void OnFSMFinished(object[] parameters)
    {
        ShowButtons();
    }

    public void ShowButtons()
    {
        transform.localPosition = new Vector3(_xShow, transform.localPosition.y, transform.localPosition.z);
    }

    public void HideButtons()
    {
        transform.localPosition = new Vector3(_xHide, transform.localPosition.y, transform.localPosition.z);
    }
}
