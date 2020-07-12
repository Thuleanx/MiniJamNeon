
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveHorizontal : MonoBehaviour
{
	Rigidbody2D body;
	ExplodeOnHit onhitExplode;

	[SerializeField] float speed;
	bool stop;

	public float Speed { get { return speed; } set { speed = value; } }

	void Awake() {
		body = GetComponent<Rigidbody2D>();
		onhitExplode = GetComponent<ExplodeOnHit>();
	}

	void OnEnable() {
		stop = false;
	}

	public void Stop() {
		stop = true;
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