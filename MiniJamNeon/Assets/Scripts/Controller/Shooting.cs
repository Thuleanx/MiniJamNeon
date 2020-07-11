using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    Camera camera;

    public float cooldown = 0.5f; // number of seconds of cooldown
    internal float curCooldown = 0;
    public GameObject[] bulletObjects;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
        curCooldown = Mathf.Max(0f, curCooldown - Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            if (curCooldown <= 0f)
			{
                curCooldown = cooldown;
                int numBullets = GameFlow.Instance.bulletCount;
                bulletObjects = new GameObject[numBullets];
                for (int i = 0; i < numBullets; i++)
                {
                    bulletObjects[i] = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y, 0.1f), Quaternion.identity);
                    GameObject bulletObj = bulletObjects[i];
                    Bullet bullet = (Bullet) bulletObj.GetComponent<Bullet>();
                    Vector3 direction = Quaternion.Euler(0, 0, (i - numBullets / 2) * 10) * (mousePosition - gameObject.transform.position);
                    bullet.setDirection(direction);
                    bullet.setSpeed(GameFlow.Instance.bulletSpeed);
                }
            }
        }
    }
    
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
