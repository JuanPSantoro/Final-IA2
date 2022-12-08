using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField]
    private Light _dayLight;
    [SerializeField]
    private Light _nightLight;

    [SerializeField]
    private GameObject _sun = default;

    [SerializeField]
    private TimeUI _timeUI = default;

    [SerializeField, Range(0, 24)]
    private float _timeOfDay = 12;

    private float _secondsPerMinute = 60;
    private float _secondsPerHour;
    private float _secondsPerDay;

    public float timeMultiplier = 1;

    private bool _nighttimeTriggered;
    private bool _daytimeTriggered;

    private void Start()
    {
        _secondsPerHour = _secondsPerMinute * 60;
        _secondsPerDay = _secondsPerHour * 24;

        UpdateSunRotation();
        UpdateTimeUI();
    }

    void Update()
    {
        _timeOfDay += (Time.deltaTime / _secondsPerHour) * timeMultiplier;

        if (_timeOfDay >= 24)
        {
            _timeOfDay = 0;
        }

        UpdateSunRotation();
        UpdateTimeUI();
    }

    private void UpdateSunRotation()
    {
        if (!_daytimeTriggered && _timeOfDay > 6 && _timeOfDay < 7)
        {
            _daytimeTriggered = true;
            EventManager.instance.TriggerEvent(EventType.NIGHT_TIME_END);
        }

        if (!_nighttimeTriggered && _timeOfDay > 18 && _timeOfDay < 19)
        {
            _nighttimeTriggered = true;
            EventManager.instance.TriggerEvent(EventType.NIGHT_TIME_START);
        }

        if (_timeOfDay > 10 && _timeOfDay < 11)
        {
            _nighttimeTriggered = false;
            _daytimeTriggered = false;
        }

        float xRot = Mathf.Lerp(0f, 360f, _timeOfDay / 24f);
        Vector3 eulerAngles = _sun.transform.localRotation.eulerAngles;
        eulerAngles.z = xRot;
        _sun.transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    private void UpdateTimeUI()
    {
        _timeUI.UpdateClock(_timeOfDay);
    }

    [ExecuteInEditMode]
    public void UpdateSun()
    {
        UpdateSunRotation();
        UpdateTimeUI();
    }
}
