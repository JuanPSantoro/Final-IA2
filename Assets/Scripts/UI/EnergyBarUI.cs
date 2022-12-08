using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{    
    [SerializeField]
    private Slider _fill = default;

    private void Start()
    {
        EventManager.instance.AddEventListener(EventType.STAMINA_CHANGE, OnStaminaChange);
    }

    private void OnStaminaChange(object[] parameters)
    {
        float percentage = (float)parameters[0];
        _fill.value = percentage;
    }
}
