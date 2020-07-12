using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingRanged : MonoBehaviour
{
    public Transform target;

    public float shootRate = 1f;

    // number tiles away that ai can shoot from
    // zero range means melee
    public float range = 8f;

    public GameObject bulletPrefab;

    private void Start()
    {
        // if range is not zero, then start shooting
        if (range != 0)
        {
            InvokeRepeating("ShootIfPossible", 1, 1f / shootRate);
        }
    }

    private void ShootIfPossible()
    {
        Debug.Log("start shoot");
        if (target == null)
        {
            //TODO: Insert a player search here
            Debug.LogError("why this here");
            return;
        }
        // distance greater than range
        if (Vector2.Distance(target.transform.position, transform.position) > range)
        {
            Debug.Log("outside range");
            return;
        }
        shootBullet();
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position, range, whatToHit);
        //Debug.DrawLine(transform.position, target.position);
        //// if raycast hits player
        //if (hit.collider != null)
        //{
        //    shootBullet();
        //}


        return;
    }

    private void shootBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Enemy has no bullet!");
            return;
        }
        GameObject bulletObj = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, 0.1f), Quaternion.identity);
        Bullet bullet = (Bullet)bulletObj.GetComponent<Bullet>();
        bullet.setDirection(target.transform.position);
        bullet.setSpeed(GameFlow.Instance.bulletSpeed);
    }
}
