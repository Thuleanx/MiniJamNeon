using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private WalkingEnemy enemy;

    private float idleTimer = 0;
    private float idleDuration;

    public void Enter(WalkingEnemy enemy)
    {
        this.enemy = enemy;

        this.idleDuration = Random.Range(enemy.minIdleDuration, enemy.maxIdleDuration);
        
    }

    public void Execute()
    {
        Idle();
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

    }

    private void Idle()
    {
        enemy.SetSpeed(0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    
}
