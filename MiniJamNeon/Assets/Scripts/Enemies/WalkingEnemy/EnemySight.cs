using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    //[SerializeField]
    private WalkingEnemy enemy;

    private void Start()
    {
        enemy = transform.parent.gameObject.GetComponent<WalkingEnemy>();
        if (enemy == null)
        {
            Debug.LogError("no walking enemy found on parent!");
        }
        //Debug.Log("found parent: " + enemy);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.Target = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.Target = null;
        }
    }
}
