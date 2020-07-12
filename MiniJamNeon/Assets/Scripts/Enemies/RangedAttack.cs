﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public Transform target;

    private SpriteRenderer spriteRenderer;

    public float shootRate = 1f;

    // number tiles away that ai can shoot from
    // zero range means melee
    public float range = 8f;

    public GameObject bulletPrefab;
    public float bulletSpeed = 12f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // if range is not zero, then start shooting
        if (range != 0)
        {
            InvokeRepeating("ShootIfPossible", 1, 1f / shootRate);
        }
    }

    private void ShootIfPossible()
    {
        if (target == null)
        {
            //TODO: Insert a player search here
            Debug.LogError("Target can't be null");
            return;
        }
        // distance greater than range
        if (Vector2.Distance(target.transform.position, transform.position) > range)
        {
            return;
        }

        // flip dude
        if (target.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        shootBullet();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position, range);
        Debug.DrawLine(transform.position, target.transform.position);
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
        bullet.setDirection(target.transform.position - transform.position);
        bullet.setSpeed(bulletSpeed);
    }
}
