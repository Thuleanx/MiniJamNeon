using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastCollider2D), typeof(InputManager), typeof(Timers))]
public class PlayerController2D : MonoBehaviour
{
	#region Components
	RaycastCollider2D raycastCollider;
	InputManager input;
	Timers timers;
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

	#region Imprecisions

	[SerializeField] float inputBufferTimeSeconds;
	[SerializeField] float coyoteTimeSeconds;
	#endregion

	void Awake() {
		raycastCollider = GetComponent<RaycastCollider2D>();
		input = GetComponent<InputManager>();
		timers = GetComponent<Timers>();
	}

	void Start() {
		CalculatePhysicsConstants();

		// Init all timers
		timers.RegisterTimer("jumpBuffer", inputBufferTimeSeconds);
		timers.RegisterTimer("coyoteBuffer", coyoteTimeSeconds);
	}

	void CalculatePhysicsConstants() {
		gravity = maxJumpHeight / (timeToJumpApexSeconds * timeToJumpApexSeconds);	

		jumpVelocityMax = gravity * timeToJumpApexSeconds;
		jumpVelocityMin = Mathf.Sqrt(minJumpHeight * gravity);
	}

	void Update() {
		input.RegisterInput();

		if (raycastCollider.collisionInfo.AnyTop || raycastCollider.collisionInfo.AnyBot)
			velocity.y = 0;

		if (raycastCollider.collisionInfo.AnyBot)
			timers.StartTimer("coyoteBuffer");

		velocity.x = input.axisInput.x * moveSpeed;
		velocity.y -= gravity * Time.deltaTime;

		// Jump
		// Potential bug with releasing the jump button possibly cancelling upward momentum. Fix not needed rn
		if (input.jumpDown)
			timers.StartTimer("jumpBuffer");	

		if (timers.Active("jumpBuffer") && !timers.Expire("jumpBuffer") && 
			timers.Active("coyoteBuffer") && !timers.Expire("coyoteBuffer")) {

			velocity.y = jumpVelocityMax;	
			timers.SetActive("jumpBuffer", false);
			timers.SetActive("coyoteBuffer", false);

			// if jump button released before touching the ground
			if (!input.jump)
				velocity.y = Mathf.Min(velocity.y, jumpVelocityMin);

		} else if (input.jumpRelease)
			velocity.y = Mathf.Min(velocity.y,jumpVelocityMin);

		raycastCollider.Move(velocity * Time.deltaTime);
	}	
}
