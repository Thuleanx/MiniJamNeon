using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastCollider2D), typeof(InputManager))]
public class PlayerController2D : MonoBehaviour
{
	#region Components
	RaycastCollider2D raycastCollider;
	InputManager input;
	#endregion

	#region Rigid body
	Vector2 velocity;
	#endregion

	#region Physics Constants

	[SerializeField] float maxJumpHeight;
	[SerializeField] float minJumpHeight;
	[SerializeField] float timeToJumpApexSeconds;

	[SerializeField] float moveSpeed;

	float gravity;

	float jumpVelocityMax;
	float jumpVelocityMin;

	#endregion

	void Awake() {
		raycastCollider = GetComponent<RaycastCollider2D>();
		input = GetComponent<InputManager>();

	}

	void Start() {
		CalculatePhysicsConstants();
	}

	void CalculatePhysicsConstants() {
		gravity = maxJumpHeight / (timeToJumpApexSeconds * timeToJumpApexSeconds);	

		jumpVelocityMax = gravity * timeToJumpApexSeconds;
		jumpVelocityMin = Mathf.Sqrt(minJumpHeight * gravity);
	}

	void Update() {
		input.RegisterInput();

		if (raycastCollider.collisionInfo.AnyTop || raycastCollider.collisionInfo.AnyBot) {
			velocity.y = 0;
		}

		velocity.x = input.axisInput.x * moveSpeed;
		velocity.y -= gravity * Time.deltaTime;

		// Jump
		// Potential bug with releasing the jump button possibly cancelling upward momentum. Fix not needed rn
		if (input.jumpDown)
			velocity.y = jumpVelocityMax;
		else if (input.jumpRelease) {
			velocity.y = Mathf.Min(velocity.y, jumpVelocityMin);
		}

		raycastCollider.Move(velocity * Time.deltaTime);
	}	
}
