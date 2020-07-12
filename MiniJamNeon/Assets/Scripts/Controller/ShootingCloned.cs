		
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCloned : MonoBehaviour
{
	internal float curCooldown = 0;

	#region Components
	GunAnimationController anim;
	#endregion

	#region Bullet Info

	[SerializeField] float fireRatePerSecond;
	float cooldownTime;

	[SerializeField] GameObject source;
	[SerializeField] string bulletTag;

	#endregion

	void Awake() {
		anim = GetComponentInChildren<GunAnimationController>();
	}

	// Start is called before the first frame update
	void Start()
	{
		cooldownTime = 1f / fireRatePerSecond;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 mousePosition = GetWorldPositionOnPlane(Input.mousePosition, 0);
		curCooldown = Mathf.Max(0f, curCooldown - Time.deltaTime);

		if (Input.GetButton("Fire1"))
		{
			if (curCooldown <= 0f)
			{
				curCooldown = cooldownTime;		

				Vector2 targetRotation = mousePosition - transform.position;

				GameObject obj = ObjectPoolA.Instance.Instantiate(
					bulletTag,
					source.transform.position,
					Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(targetRotation.y, targetRotation.x))
				);

				SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
				if (renderer != null)
					renderer.flipY = targetRotation.x < 0;

				anim?.SetState(GunStates.Shoot);	
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
