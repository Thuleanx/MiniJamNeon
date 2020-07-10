using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[HideInInspector] public Vector2 axisInput;
	[HideInInspector] public bool jumpDown, jumpRelease;

	public void RegisterInput() {
		axisInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		jumpDown = Input.GetButtonDown("Jump");
		jumpRelease = Input.GetButtonUp("Jump");
	}
}

