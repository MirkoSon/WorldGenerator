using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles the Wander Behaviour of any character that has this component.
/// It determines Idle and Walking state based on random values inside ranges set in the <see cref="RulesData"/>
/// </summary>
public class WanderBehaviour : MonoBehaviour
{    
    private NavMeshAgent _agent;

    private Animator _animator;

    private int _animatorBoolHash;

    public void Initialize()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animatorBoolHash = Animator.StringToHash("Moving");
        WaitOrMoveRoll();
    }

    private void Update()
    {
        if (_agent.remainingDistance == _agent.stoppingDistance)
        {
            if (!_agent.isStopped)
                WaitOrMoveRoll();
        }
    }

    private void WaitOrMoveRoll()
    {
        _agent.isStopped = true;
        _animator.SetBool(_animatorBoolHash, false);
        int roll = Random.Range(1, 10);

        if (roll > Main.Rules.idleProbability)
        {
            GetNewDestination();
        }
        else
        {
            StartCoroutine(IdleForAWhile());
        }
    }

    private IEnumerator IdleForAWhile()
    {
        int timeRoll = Random.Range((int)Main.Rules.idleTimeRange.x, (int)Main.Rules.idleTimeRange.y);
        yield return new WaitForSeconds(timeRoll);
        GetNewDestination();
    }

    private void GetNewDestination()
    {
        _agent.isStopped = false;
        _animator.SetBool(_animatorBoolHash, true);
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(GetRandomPoint(Main.Rules.radiusRange.y), path);
        if (path.status == NavMeshPathStatus.PathComplete)
            _agent.SetDestination(GetRandomPoint(Main.Rules.radiusRange.y));
    }

    private Vector3 GetRandomPoint(float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);

        return navHit.position;
    }
}
