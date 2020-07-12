
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveHorizontal : MonoBehaviour
{
	Rigidbody2D body;

	[SerializeField] float speed;
	bool stop;

	public float Speed { get { return speed; } set { speed = value; } }

	void Awake() {
		body = GetComponent<Rigidbody2D>();
	}

	void OnEnable() {
		stop = false;
	}

	void Update()
	{
		if (!stop) {
			body.velocity = speed * (Vector2) transform.right;	
		} else {
			body.velocity = Vector2.zero;
		}
	}
}