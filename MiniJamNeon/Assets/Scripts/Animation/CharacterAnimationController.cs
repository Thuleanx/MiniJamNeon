using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
	#region Components
	Animator anim;
	#endregion

	AnimState currentState;
	public AnimState State { get { return currentState; } }

	void Start() {
		anim = GetComponent<Animator>();
		Reset();
	}	

	public void SetState(AnimState state) {
		currentState = state;
		anim.SetInteger("State", (int) state);
	}

	public void Reset() {
		SetState(AnimState.Idle);
	}
}
