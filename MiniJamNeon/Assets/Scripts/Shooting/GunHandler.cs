
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
	[SerializeField] GameObject lefthand;
	[SerializeField] GameObject righthand;

	ShootingCloned gun;

	void Start() {
		gun = GetComponentInChildren<ShootingCloned>();
	}

	void Update() {
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (mousePosition.x < transform.position.x)
			gun.transform.position = lefthand.transform.position;
		else
			gun.transform.position = righthand.transform.position;
	}
}
