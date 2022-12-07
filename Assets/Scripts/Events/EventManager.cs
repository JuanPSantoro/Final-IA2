using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EventReceiver(params object[] parameters);
    private static Dictionary<EventType, EventReceiver> events;
    public static EventManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    public void AddEventListener(EventType eventType, EventReceiver callback)
    {
        if (events == null)
            events = new Dictionary<EventType, EventReceiver>();

        if (events.ContainsKey(eventType))
        {
            events[eventType] += callback;
        }
        else
        {
            events.Add(eventType, callback);
        }
    }

    public void RemoveEventListener(EventType eventType, EventReceiver callback)
    {
        if (events != null)
        {
            if (events.ContainsKey(eventType))
            {
                events[eventType] -= callback;
            }
        }
    }

    public void RemoveAllEvents()
    {
        events = new Dictionary<EventType, EventReceiver>();
    }

    public void TriggerEvent(EventType eventType)
    {
        TriggerEvent(eventType, null);
    }

    public void TriggerEvent(EventType eventType, params object[] parametersWrapper)
    {
        if (events != null)
        {
            if (events.ContainsKey(eventType) && events[eventType] != null)
            {
                events[eventType](parametersWrapper);
            }
        }
    }
}
