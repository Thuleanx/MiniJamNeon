
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasRange : MonoBehaviour
{
	[SerializeField] float range;
	Vector2 origin;

	void OnEnable() {
		origin = transform.position;
	}

	void Update()
	{
		if (((Vector2) transform.position - origin).sqrMagnitude >= range * range)
			gameObject.SetActive(false);
	}
}