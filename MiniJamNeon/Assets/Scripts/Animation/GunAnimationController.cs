using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GunAnimationController : MonoBehaviour
{
	#region Components
	Animator anim;
	#endregion

	GunStates currentState;
	public GunStates State { get { return currentState; } }

	void Awake() {
		anim = GetComponent<Animator>();
	}

	void Start() {
		Reset();
	}	

	public void SetState(GunStates state) {
		currentState = state;
		anim.SetInteger("State", (int) state);
		if (currentState == GunStates.Shoot) {
			AudioManager.Instance.Play("Gunshot");
		}
		if (currentState == GunStates.Reload)
			AudioManager.Instance.Play("Reload");
	}

	public void Reset() {
		SetState(GunStates.Idle);
	}
}
