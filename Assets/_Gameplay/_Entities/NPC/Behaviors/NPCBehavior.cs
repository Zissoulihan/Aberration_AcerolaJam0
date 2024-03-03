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
        OnEnterBehavior?.Invoke(_state);
    }

    public virtual void Tick(float tickDelta)
    {
        _behaviorTimeSpent += tickDelta;
    }

    public virtual void ExitBehavior()
    {
        OnExitBehavior?.Invoke(_state);
    }
}
