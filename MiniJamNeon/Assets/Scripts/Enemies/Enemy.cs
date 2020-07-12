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

    //private Collider2D collider;

    private void Start()
    {
        //collider = GetComponent<Collider2D>();
        //Physics2D.IgnoreLayerCollision()
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    Physics2D.IgnoreCollision(collider, collision.gameObject.collider);
        //}
    }


}
