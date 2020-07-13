using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private WalkingEnemy enemy;

    public void Enter(WalkingEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        if (enemy.Target != null)
        {
            if (enemy.isRanged)
            {
                enemy.SetSpeed(0);
            }
            else
            {
                enemy.Move();
            }
            
            
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
            enemy.Target = null;
        }
    }

    
}
