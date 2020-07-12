using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int Health = 100;
    }

    public EnemyStats stats = new EnemyStats();

    public void DamageEnemy (int damage)
    {
        stats.Health -= damage;
        if (stats.Health <= 0)
        {
            //TODO: Centralized destruction of enemies?
            // GameMaster.KillEnemy(this);
            Destroy(gameObject);
            Debug.Log("I died");
        }
    }
}
