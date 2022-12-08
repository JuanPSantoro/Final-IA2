using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField]
    private Text _text = default;
    [SerializeField]
    private EventType _eventToListen = default;

    private void Start()
    {
        EventManager.instance.AddEventListener(_eventToListen, OnResourceChange);
    }

    private void OnResourceChange(object[] parameters)
    {
        UpdateElement((int)parameters[0]);
    }

    public void UpdateElement(int newValue)
    {
        _text.text = newValue.ToString();
    }
}
