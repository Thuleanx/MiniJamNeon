using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExplodeOnHit : MonoBehaviour
{
	Animator anim;
	MoveHorizontal mover;
	ScreenShakeController shakeController;

	void Awake() {
		anim = GetComponent<Animator>();
		mover = GetComponent<MoveHorizontal>();
	}

	void OnEnable() {
		anim.SetInteger("State", 0);
	}

	public void Explode() {
		anim.SetInteger("State", 1);
		mover?.Stop();

		ScreenShakeController.instance.StartShake(1f, .005f);
	}

    private void OnTriggerEnter2D(Collider2D collision) {
		// thanks Eliz
		Explode();
    }
}
