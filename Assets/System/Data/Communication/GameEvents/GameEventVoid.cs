using System;
using UnityEngine;

[CreateAssetMenu(fileName ="ev",menuName ="Data/Events/GameEvent")]
public class GameEventVoid : ScriptableObject
{
    protected virtual event Action _event;

    public virtual void Subscribe(Action subscribingMethod)
    {
        _event += subscribingMethod;
    }
    public virtual void Unsubscribe(Action subscribedMethod)
    {
        _event -= subscribedMethod;
    }
    public virtual void TriggerEvent()
    {
        _event?.Invoke();
    }
}