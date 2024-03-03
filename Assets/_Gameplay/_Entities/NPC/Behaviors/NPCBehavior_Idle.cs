using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior_Idle : NPCBehavior
{
    [SerializeField] float _rateCheckForPlayer;

    float _timeLastPlayerCheck;
    bool _playerDetected;

    public override void Tick(float tickDelta)
    {
        base.Tick(tickDelta);

        CheckForPlayer();
    }
    void CheckForPlayer()
    {
        if (_playerDetected) return;
        if (Time.time < _timeLastPlayerCheck + _rateCheckForPlayer) return;
        _timeLastPlayerCheck = Time.time;
        if (!_sense.LookForPlayer()) return;

        _playerDetected = true;

        //TODO: Just flee/panic sometimes
        _actor.ChangeBehavior(NPCState.Fear);
    }
}
