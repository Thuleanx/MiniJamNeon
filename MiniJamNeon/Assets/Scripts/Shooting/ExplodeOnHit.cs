using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExplodeOnHit : MonoBehaviour
{
	Animator anim;
	MoveHorizontal mover;
	ScreenShakeController shakeController;
	Hitbox hitbox;

	void Start() {
		anim = GetComponent<Animator>();
		hitbox = GetComponentInChildren<Hitbox>();
		mover = GetComponent<MoveHorizontal>();
	}

	void OnEnable() {
		anim?.SetInteger("State", 0);
		hitbox?.gameObject.SetActive(true);
	}

	public void Explode() {
		anim.SetInteger("State", 1);
		mover?.Stop();
		hitbox?.gameObject.SetActive(false);

		ScreenShakeController.instance.StartShake(1f, .005f);
	}

    private void OnTriggerEnter2D(Collider2D collision) {
		// thanks Eliz
		Explode();
    }
}
