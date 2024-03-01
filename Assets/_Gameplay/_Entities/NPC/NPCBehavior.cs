using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] NPCSensory _sense;
    [Header("Idle")]
    [SerializeField] float _rateCheckPlayer;
    [Header("Fear")]
    [SerializeField] float _durationShock;

    NPCMovement _move;

    bool _playerDetected;
    bool _hidingSpotFound;

    float _timeLastPlayerCheck;
    float _timeLastPlayerSeen;
    
    private void Awake()
    {
        Init();
    }

    void Init()
    {
        _move = GetComponentInChildren<NPCMovement>();
    }

    private void Update()
    {
        CheckForPlayer();
        FindHidingSpot();
    }

    void CheckForPlayer()
    {
        if (_playerDetected) return;
        if (Time.time < _timeLastPlayerCheck + _rateCheckPlayer) return;
        _timeLastPlayerCheck = Time.time;
        if (!_sense.LookForPlayer()) return;

        print($"Oh sweet jesus fuck, run!");
        _playerDetected = true;
        _timeLastPlayerSeen = Time.time;
    }

    void FindHidingSpot()
    {
        if (!_playerDetected) return;
        if (_hidingSpotFound) return;
        if (Time.time < _timeLastPlayerSeen + _durationShock) return;

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
