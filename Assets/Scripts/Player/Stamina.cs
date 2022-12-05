using IA2;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    private float _energyToRecoverPerRest;

    [SerializeField]
    private float _maxEnergy;
    private float _currentEnergy;

    [SerializeField]
    private UnityEvent<float> _onEnergyUpdate;

    public float CurrentEnergy { get { return _currentEnergy; } }

    public void Rest()
    {
        StartCoroutine(DoRest());
    }

    public void Sleep()
    {
        _currentEnergy = _maxEnergy;
        StartCoroutine(DoSleep());
    }

    public void ConsumeEnergy(float energy)
    {
        _currentEnergy -= energy;
        _onEnergyUpdate?.Invoke(_currentEnergy);
    }

    private IEnumerator DoSleep()
    {
        yield return new WaitForSeconds(2); //Replace with animation values
        _currentEnergy = _maxEnergy;
        _onEnergyUpdate?.Invoke(_currentEnergy);
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    private IEnumerator DoRest()
    {
        yield return new WaitForSeconds(3); //Replace with Idle Value
        _currentEnergy += _energyToRecoverPerRest;
        _onEnergyUpdate?.Invoke(_currentEnergy);
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }
}
