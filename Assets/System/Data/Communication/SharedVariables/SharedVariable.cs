using UnityEngine;
using Sirenix.OdinInspector;

public class SharedVariable<T> : ScriptableObject
{
    protected T _value;
    [ShowInInspector] public virtual T Value => _value;
    
    public virtual void Set(T val)
    {
        _value = val;
    }
}
