using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior_Fear : NPCBehavior
{
    [SerializeField] Vector2 _rangeDurationCowerInFear;

    float _durationFear;

    public override void EnterBehavior()
    {
        base.EnterBehavior();
        _durationFear = Random.Range(_rangeDurationCowerInFear.x, _rangeDurationCowerInFear.y);
    }
    public override void Tick(float tickDelta)
    {
        base.Tick(tickDelta);

        CowerInFear();
    }

    void CowerInFear()
    {
        if (_behaviorTimeSpent < _durationFear) return;

        //Transition
        _actor.ChangeBehavior(NPCState.Flee);
    }
    
}
