using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class NPCBehavior : MonoBehaviour
{

    public UnityEvent<NPCState> OnEnterBehavior;
    public UnityEvent<NPCState> OnExitBehavior;

    protected NPCActor _actor;
    protected NPCMovement _move;
    protected NPCSensory _sense;

    protected NPCState _state;

    protected float _behaviorTimeSpent;

    protected bool _exitingBehavior;

    public virtual void Initialize(NPCActor actor, NPCMovement movement, NPCSensory sensory, NPCState state)
    {
        _actor = actor;
        _move = movement;
        _sense = sensory;
        _state = state;
    }
    public virtual void EnterBehavior()
    {
        _behaviorTimeSpent = 0f;
        _exitingBehavior = false;
        OnEnterBehavior?.Invoke(_state);
    }

    public virtual void Tick(float tickDelta)
    {
        if (_exitingBehavior) return;
        _behaviorTimeSpent += tickDelta;
    }

    public virtual void ExitBehavior()
    {
        OnExitBehavior?.Invoke(_state);
        _exitingBehavior = true;
    }
}
