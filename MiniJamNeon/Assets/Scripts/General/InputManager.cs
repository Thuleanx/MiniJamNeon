using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[HideInInspector] public Vector2 axisInput;
	[HideInInspector] public bool jumpDown, jumpRelease, jump, dash;

	public void RegisterInput() {
		axisInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		jumpDown = Input.GetButtonDown("Jump");
		jump = Input.GetButton("Jump");
		jumpRelease = Input.GetButtonUp("Jump");
		dash = Input.GetButtonDown("Dash");
	}
}

