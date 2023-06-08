using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    public List<Transform> PatrolPoints;

    private Transform _goal;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Vector3 _worldDeltaPos;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private int _indexOfPatrolPoint;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.updatePosition = false;
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _indexOfPatrolPoint = 0;
        _goal = PatrolPoints[_indexOfPatrolPoint];
    }

    void Update()
    {
        if (Mathf.Approximately(transform.position.x, _goal.position.x) && Mathf.Approximately(transform.position.z, _goal.position.z))
        {
            _indexOfPatrolPoint++;

            if (PatrolPoints.Count <= _indexOfPatrolPoint)
            {
                _indexOfPatrolPoint = 0;
            }

            _goal = PatrolPoints[_indexOfPatrolPoint];
        }

        Vector3 direction = (_goal.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        _agent.destination = _goal.position;
        _worldDeltaPos = _agent.nextPosition - transform.position;
        bool shouldMove = _worldDeltaPos.magnitude != 0;
        _animator.SetFloat(_animIDSpeed, shouldMove ? 1 : 0);
        _animator.SetFloat(_animIDMotionSpeed, shouldMove ? 0.8f : 0);

        transform.position = _agent.nextPosition;

    }
}
