using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEmitter : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    [SerializeField]
    private EventType _eventToPlayParticles = default;
    [SerializeField]
    private EventType _eventToStopParticles = default;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop(true);
        EventManager.instance.AddEventListener(_eventToPlayParticles, OnParticlePlay);
        EventManager.instance.AddEventListener(_eventToStopParticles, OnParticleStop);
    }

    private void OnParticleStop(object[] parameters)
    {
        _particleSystem.Stop(true);
    }

    private void OnParticlePlay(object[] parameters)
    {
        transform.position = (Vector3)parameters[0];
        _particleSystem.Play();
    }
}
