using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent<T> : ScriptableObject
{
    protected virtual event Action<T> _event;

    public virtual void Subscribe(Action<T> subscribingMethod)
    {
        _event += subscribingMethod;
    }
    public virtual void Unsubscribe(Action<T> subscribedMethod)
    {
        _event -= subscribedMethod;
    }
    public virtual void TriggerEvent(T eventData)
    {
        _event?.Invoke(eventData);
    }

    [ContextMenu("DEBUG Print Subscribers")]
    public virtual void DebugPrintSubscribers()
    {
        int count = 0;
        foreach (var sub in _event.GetInvocationList()) {
            Debug.Log($"{name} Delegate {count++}: {sub.Method}");
        }
    }

}