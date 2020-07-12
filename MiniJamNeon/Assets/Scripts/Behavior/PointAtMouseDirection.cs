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

		transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(target.y, target.x));	
	}
}
