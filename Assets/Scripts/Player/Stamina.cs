using IA2;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    private float _energyToRecoverPerRest = 1;

    [SerializeField]
    private float _maxEnergy = 100;
    private float _currentEnergy = 100;

    [SerializeField]
    private UnityEvent<float> _onEnergyUpdate;

    [SerializeField]
    private float _restingTime = 2;

    private bool _resting;
    private float _energyOnRestStart;
    private float _energyOnRestEnd;
    private float _timer;

    [SerializeField]
    private AudioClipSO _sleepAudio = default;

    public float CurrentEnergy { get { return _currentEnergy; } }

    private void Awake()
    {
        _currentEnergy = _maxEnergy;
    }

    private void Start()
    {
        EventManager.instance.AddEventListener(EventType.STAMINA_SPENT, OnStaminaSpent);
    }

    private void OnStaminaSpent(object[] parameters)
    {
        _currentEnergy -= (float)parameters[0];
        EventManager.instance.TriggerEvent(EventType.STAMINA_CHANGE, new object[] { _currentEnergy / _maxEnergy });
    }

    private void Update()
    {
        if (_resting)
        {
            _timer += Time.deltaTime;
            _currentEnergy = Mathf.Lerp(_energyOnRestStart, _energyOnRestEnd, _timer /  _restingTime);
            EventManager.instance.TriggerEvent(EventType.STAMINA_CHANGE, new object[] { _currentEnergy / _maxEnergy });
        }
    }

    public void Rest()
    {
        StartCoroutine(DoRest());
    }

    public void Sleep()
    {
        SoundManager.instance.PlayOnPosition(_sleepAudio, transform.position);
        _currentEnergy = _maxEnergy;
        EventManager.instance.TriggerEvent(EventType.SLEEP_PARTICLE_PLAY, new object[] { transform.position });
        EventManager.instance.AddEventListener(EventType.NIGHT_TIME_END, OnNightTimeEnd);
    }

    private void OnNightTimeEnd(object[] parameters)
    {
        SoundManager.instance.Stop();
        EventManager.instance.RemoveEventListener(EventType.NIGHT_TIME_END, OnNightTimeEnd);
        EventManager.instance.TriggerEvent(EventType.SLEEP_PARTICLE_STOP);
        _currentEnergy = _maxEnergy;
        EventManager.instance.TriggerEvent(EventType.STAMINA_CHANGE, new object[] { _currentEnergy / _maxEnergy });
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    private IEnumerator DoRest()
    {
        _timer = 0;
        _energyOnRestStart = _currentEnergy;
        _energyOnRestEnd = Math.Min(_currentEnergy + _energyToRecoverPerRest, _maxEnergy);
        _resting = true;
        yield return new WaitForSeconds(_restingTime);
        _resting = false;
        _currentEnergy = _energyOnRestEnd;
        EventManager.instance.TriggerEvent(EventType.STAMINA_CHANGE, new object[] { _currentEnergy / _maxEnergy });
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }
}
