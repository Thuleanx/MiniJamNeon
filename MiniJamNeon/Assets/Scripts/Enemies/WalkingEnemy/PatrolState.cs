using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private WalkingEnemy enemy;
    private float patrolTimer;
    private float patrolDuration;

    public void Enter(WalkingEnemy enemy)
    {
        this.enemy = enemy;
        this.patrolDuration = Random.Range(enemy.minPatrolDuration, enemy.maxPatrolDuration);
    }

    public void Execute()
    {
        Patrol();
        enemy.Move();
        if (enemy.Target != null)
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
        // Debug.Log("triggered");
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
    }

    private void Patrol()
    {

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }

}
