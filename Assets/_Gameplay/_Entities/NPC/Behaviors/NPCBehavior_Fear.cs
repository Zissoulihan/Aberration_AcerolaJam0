using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior_Fear : NPCBehavior
{
    [SerializeField] float _durationShock;

    bool _hidingSpotFound;

    public override void Tick(float tickDelta)
    {
        base.Tick(tickDelta);

        FindHidingSpot();
    }

    void FindHidingSpot()
    {
        if (_hidingSpotFound) return;
        if (_behaviorTimeSpent < _durationShock) return;

        if (NPCHidingSpot.HidingSpots == null || NPCHidingSpot.HidingSpots.Count == 0) {
            //TODO: Panik
            return;
        }

        float minDist = 999f;
        NPCHidingSpot goalSpot = null;
        foreach (var spot in NPCHidingSpot.HidingSpots) {
            if (spot == null || spot.Claimed) continue;
            float distToSpot = Vector3.Distance(_move.transform.position, spot.HidingPosition);
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
        _move.MoveToGoal(goalSpot.HidingPosition);
        _hidingSpotFound = true;
    }
}
