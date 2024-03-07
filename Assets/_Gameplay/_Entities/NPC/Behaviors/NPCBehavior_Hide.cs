using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior_Hide : NPCBehavior
{
    [SerializeField] Vector2 _rangeDurationHide;
    [SerializeField] float _minSafePlayerDistance;

    float _durationHide;

    public override void EnterBehavior()
    {
        base.EnterBehavior();
        _durationHide = Random.Range(_rangeDurationHide.x, _rangeDurationHide.y);
    }

    public override void Tick(float tickDelta)
    {
        base.Tick(tickDelta);

        ConsiderSafety();
    }

    void ConsiderSafety()
    {
        if (_behaviorTimeSpent < _durationHide) return;
        if (_sense.GetDistanceFromPlayer() < _minSafePlayerDistance) return;
        _actor.LeaveHidingSpot();
        _actor.ChangeBehavior(NPCState.Idle);
    }
}
