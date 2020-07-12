using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
