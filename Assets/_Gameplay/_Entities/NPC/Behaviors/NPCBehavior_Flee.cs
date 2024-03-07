using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior_Flee : NPCBehavior
{
    [SerializeField] float _maxHideSpotDistance;
    [SerializeField] float _distanceConsiderSafe;

    bool _hidingSpotFound;
    bool _fleeingToSpot;

    public override void Tick(float tickDelta)
    {
        base.Tick(tickDelta);

        ConsiderSafety();
        FindHidingSpot();
        Flee();
    }

    void ConsiderSafety()
    {
        if (_exitingBehavior) return;
        if (_sense.GetDistanceFromPlayer() < _distanceConsiderSafe) return;

        //TODO: Remember to stop current movement
        _actor.ChangeBehavior(NPCState.Idle);
    }

    void FindHidingSpot()
    {
        if (_exitingBehavior) return;
        if (_hidingSpotFound) return;

        if (NPCHidingSpot.HidingSpots == null || NPCHidingSpot.HidingSpots.Count == 0) {
            //TODO: Panik
            return;
        }

        //Find closest unoccupied hiding space within range
        float minDist = 999f;
        NPCHidingSpot goalSpot = null;
        foreach (var spot in NPCHidingSpot.HidingSpots) {
            if (spot == null || spot.Claimed) continue;
            float distToSpot = Vector3.Distance(_move.transform.position, spot.HidingPosition);
            if (distToSpot > _maxHideSpotDistance) continue;
            if (distToSpot < minDist) {
                goalSpot = spot;
                minDist = distToSpot;
            }
        }
        if (goalSpot == null) {
            print("Hiding spots searched but none could be found!");
            return;
        }

        goalSpot.CallDibs();
        _actor.HideInSpot(goalSpot);
        _move.MoveToGoal(goalSpot.HidingPosition);
        _move.OnReachGoal += Hide;      //TODO: Remember this when it fails
        _hidingSpotFound = true;
    }

    void Flee()
    {
        if (_exitingBehavior) return;
        if (_hidingSpotFound) return;
        //Run away from player
    }

    void Hide()
    {
        _move.OnReachGoal -= Hide;
        _actor.ChangeBehavior(NPCState.Hide);
    }
}
