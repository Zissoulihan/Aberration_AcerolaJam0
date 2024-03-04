using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] float _distanceConsiderGoalReached;
    [SerializeField] SharedVariableBool _svGamePaused;

    public event Action OnReachGoal;

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
        _nav = GetComponent<NavMeshAgent>();
    }

    public void MoveToGoal(Vector3 goal)
    {
        GoalPos = goal;
        _nav.destination = goal;
        _movingToGoal = true;
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
