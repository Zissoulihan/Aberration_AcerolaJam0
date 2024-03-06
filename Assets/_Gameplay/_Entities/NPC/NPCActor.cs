using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActor : SerializedMonoBehaviour
{
    [SerializeField] NPCMovement _move;
    [SerializeField] NPCSensory _sense;
    [SerializeField] NPCAnimator _anim;
    [OdinSerialize] Dictionary<NPCState, NPCBehavior> _behaviors;
    [SerializeField] float _tickRate;
    [SerializeField] SharedVariableBool _svGamePaused;

    public Dictionary<NPCState, NPCBehavior> Behaviors => _behaviors;

    NPCBehavior _activeBehavior;

    float _tickTimer;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        foreach (var kvp in _behaviors) {
            kvp.Value.Initialize(this, _move, _sense, kvp.Key);
        }

        _activeBehavior = _behaviors[NPCState.Idle];
        _activeBehavior.EnterBehavior();
    }

    public void ChangeBehavior(NPCState newState)
    {
        if (!_behaviors.ContainsKey(newState)) {
            print($"You forgot to add NPC {newState} behavior to dictionary!");
            return;
        }
        if (_activeBehavior is not null) _activeBehavior.ExitBehavior();
        _activeBehavior = _behaviors[newState];
        _activeBehavior.EnterBehavior();
        _anim.AnimState(newState);
    }

    public void HaveHeartAttack()
    {
        print("BLEFHFHSSHHHGGH!");
    }

    private void Update()
    {
        UpdateTickTimer();
        BehaviorTick();
    }

    void BehaviorTick()
    {
        if (_activeBehavior is null) return;
        if (_tickTimer < _tickRate) return;

        _activeBehavior.Tick(_tickTimer);
        _tickTimer = 0f;
    }

    void UpdateTickTimer()
    {
        if (_activeBehavior is null) return;
        if (_svGamePaused.Value) return;
        _tickTimer += Time.deltaTime;
    }

}

public enum NPCState
{
    Idle,
    Fear,
    Flee,
    Hide,
    Seek,
    Interact,
    Escape,
    Pain,
    Dead,
    ENUM_HEIGHT
}