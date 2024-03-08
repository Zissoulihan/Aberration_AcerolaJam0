using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] NPCSensory _sense;
    [SerializeField] float _distanceConsiderGoalReached;
    [SerializeField] float _distanceTryFlee;
    [SerializeField] int _maxNumFleeAttempts;
    [SerializeField] float _fleeDirAngleVariance = 45f;
    [SerializeField] SharedVariableBool _svGamePaused;

    public event Action OnReachGoal;

    Transform _body;
    NavMeshAgent _nav;

    public Vector3 GoalPos { get; private set; }

    bool _movingToGoal;
    bool _movePaused;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        _body = GetComponent<Transform>();
        _nav = GetComponent<NavMeshAgent>();
    }

    public void MoveToGoal(Vector3 goal)
    {
        GoalPos = goal;
        _nav.destination = goal;
        _movingToGoal = true;
    }

    public bool TryFlee(Vector3 directionToFlee)
    {
        //Can flee in supplied direction?
        Vector3 goalPos = _body.position + (directionToFlee * _distanceTryFlee);
        if (!_sense.IsWallBlocking(directionToFlee, _distanceTryFlee)) {
            MoveToGoal(goalPos);
            return true;
        }

        float angleIncrement = (_fleeDirAngleVariance * 2) / _maxNumFleeAttempts;
        Vector3 fleeDir = Quaternion.AngleAxis(-_fleeDirAngleVariance, Vector3.up) * directionToFlee;
        for (int i = 0; i < _maxNumFleeAttempts; i++) {
            fleeDir = Quaternion.AngleAxis(i * angleIncrement, Vector3.up) * fleeDir;
            if (_sense.IsWallBlocking(fleeDir, _distanceTryFlee)) continue;
            //Pos is not blocked by wall
            MoveToGoal(_body.position + (fleeDir * _distanceTryFlee));
            return true;
        }

        //Everything is walls - you're fucked lol
        return false;
    }

    private void Update()
    {
        HandlePausing();
        CheckMoveProgress();
    }

    void HandlePausing()
    {
        if (!_movingToGoal) return;
        if (_svGamePaused.Value) {
            if (_movePaused) return;
            _nav.isStopped = true;
            _movePaused = true;
        }
        else {
            if (!_movePaused) return;
            _nav.isStopped = false;
            _movePaused = false;
        }
    }

    void CheckMoveProgress()
    {
        if (!_movingToGoal) return;
        if (!_nav.hasPath) return;
        if (_nav.remainingDistance > _distanceConsiderGoalReached) return;
        OnReachGoal?.Invoke();
        _movingToGoal = false;
    }
}
