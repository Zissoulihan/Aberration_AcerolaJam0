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

    float _timeLastPlayerCheck;

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
    }

    void CheckForPlayer()
    {
        if (_playerDetected) return;
        if (Time.time < _timeLastPlayerCheck + _rateCheckPlayer) return;
        _timeLastPlayerCheck = Time.time;
        if (!_sense.LookForPlayer()) return;

        print($"Oh sweet jesus fuck, run!");
        _playerDetected = true;
    }

}
