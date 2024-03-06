using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCFear : MonoBehaviour
{
    [SerializeField] NPCSensory _sense;
    [SerializeField] float _maxFear;
    [SerializeField] float _fearAmtOnPlayerSeen;
    [SerializeField] float _fearAmtOnPlayerNear;
    [SerializeField] float _rateCheckPlayerSeen;
    [SerializeField] float _rateCheckPlayerDistance;
    [SerializeField] float _distanceFearPlayer;
    [SerializeField] SharedVariableBool _svGamePaused;
    [SerializeField] SharedVariableVector3 _svPlayerPos;

    public UnityEvent OnMaxFearReached;
    public float FearValue => Mathf.Clamp(_currentFear, 0f, _maxFear);

    float _currentFear;

    float _timerSightCheck;
    float _timerDistanceCheck;

    bool _maxFearReached;

    private void Update()
    {
        if (_svGamePaused.Value) return;
        UpdateTimers();
        CheckPlayerSeen();
        CheckPlayerDistance();
    }

    void UpdateTimers()
    {
        _timerSightCheck += Time.deltaTime;
        _timerDistanceCheck += Time.deltaTime;
    }

    void CheckPlayerSeen()
    {
        if (_timerSightCheck < _rateCheckPlayerSeen) return;
        _timerSightCheck = 0f;
        if (!_sense.LookForPlayer()) return;
        AddFear(_fearAmtOnPlayerSeen);
    }
    void CheckPlayerDistance()
    {
        if (_timerDistanceCheck < _rateCheckPlayerDistance) return;
        _timerDistanceCheck = 0f;
        if (Mathf.Abs(Vector3.Distance(_sense.transform.position, _svPlayerPos.Value)) > _distanceFearPlayer) return;
        AddFear(_fearAmtOnPlayerNear);
    }

    public void AddFear(float fearAmt)
    {
        if (_maxFearReached) return;
        _currentFear += fearAmt;

        if (_currentFear < _maxFear) return;
        OnMaxFearReached?.Invoke();
        _maxFearReached = true;
    }
}
