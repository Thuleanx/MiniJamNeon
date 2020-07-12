using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtMouseDirection : MonoBehaviour
{
	Camera mainCamera; 

	void Awake() {
		mainCamera = Camera.main;
	}

	void Update() {
		Vector2 target = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		if (target.x < 0) {
			if (transform.localScale.y > 0)
				Flip();
		} else {
			if (transform.localScale.y < 0) 
				Flip();
		}
		transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(target.y, target.x));	
	}

	void Flip() {
		transform.localScale = new Vector3(
			transform.localScale.x,
			-transform.localScale.y,
			transform.localScale.z
		);
	}
}
