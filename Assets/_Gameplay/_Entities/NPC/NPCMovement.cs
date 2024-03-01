using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    
    NavMeshAgent _nav;

    public Vector3 GoalPos { get; private set; }

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
    }
}
