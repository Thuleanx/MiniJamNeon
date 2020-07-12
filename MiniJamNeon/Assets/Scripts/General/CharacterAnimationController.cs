using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
	public enum AnimState {
		Idle = 0,
		Run = 1,
		Fall = 2,
		Jump = 3,
		Attack = 4,
		Hurt = 5
	}	

	#region Components
	Animator anim;
	#endregion

	AnimState currentState;

	void Start() {
		Reset();
	}	

	void ChangeState(AnimState state) {
		currentState = state;
		anim.SetInteger("State", (int) state);
	}

	void Reset() {
		ChangeState(AnimState.Idle);
	}
}
