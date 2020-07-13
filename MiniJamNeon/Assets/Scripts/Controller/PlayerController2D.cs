﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RaycastCollider2D), typeof(InputManager), typeof(Timers))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController2D : MonoBehaviour
{
	#region Components
	RaycastCollider2D raycastCollider;
	InputManager input;
	Timers timers;
	PlayerStats stats;
	CharacterAnimationController anim;
	SpriteRenderer sprite;

	#endregion

	#region Rigid body
	Vector2 velocity;
	#endregion

	#region Physics Constants

	[SerializeField] float maxJumpHeight;
	[SerializeField] float minJumpHeight;
	[SerializeField] float timeToJumpApexSeconds;

	[SerializeField] float moveSpeed;
	[SerializeField] float dashDistance;
	[SerializeField] float wallSlideSpeed;
	[SerializeField] float terminalVelocity = 10;

	float gravity;

	float jumpVelocityMax;
	float jumpVelocityMin;

	#endregion

	#region Imprecisions

	[SerializeField] float inputBufferTimeSeconds;
	[SerializeField] float coyoteTimeSeconds;
	[SerializeField] float dashCooldownSeconds;
	float platformFallThroughSeconds;
	#endregion

	int currMoney;


	// face direction to animate the sprite
	int faceDir;

	void Awake() {
		raycastCollider = GetComponent<RaycastCollider2D>();
		input = GetComponent<InputManager>();
		timers = GetComponent<Timers>();
    	stats = GetComponent<PlayerStats>();
		sprite = GetComponent<SpriteRenderer>();
		anim = GetComponent<CharacterAnimationController>();
	}

	void Start() {
		CalculatePhysicsConstants();
    	currMoney = 0;

		// Init all timers
		timers.RegisterTimer("jumpBuffer", inputBufferTimeSeconds);
		timers.RegisterTimer("coyoteBuffer", coyoteTimeSeconds);
		timers.RegisterTimer("platformFallThrough", platformFallThroughSeconds);
		timers.RegisterTimer("dashBuffer", dashCooldownSeconds);
	}

	void CalculatePhysicsConstants() {
		gravity = 2 * maxJumpHeight / (timeToJumpApexSeconds * timeToJumpApexSeconds);	

		jumpVelocityMax = gravity * timeToJumpApexSeconds;
		jumpVelocityMin = Mathf.Sqrt(minJumpHeight * gravity);

		platformFallThroughSeconds =  Mathf.Sqrt(1f / gravity);
	}

	void Update() {
		input.RegisterInput();

		#region Movement

		float oldVelocityX = velocity.x;

		if (raycastCollider.collisionInfo.AnyBot || raycastCollider.platformCollisionInfo.AnyBot)
			timers.StartTimer("coyoteBuffer");


		velocity.x = input.axisInput.x * moveSpeed;
		velocity.y -= gravity * Time.deltaTime;

		if (raycastCollider.collisionInfo.AnyBot || raycastCollider.platformCollisionInfo.AnyBot || raycastCollider.collisionInfo.AnyTop)
			velocity.y = 0;

		// Jump
		// Potential bug with releasing the jump button possibly cancelling upward momentum. Fix not needed rn
		if (input.jumpDown)
			timers.StartTimer("jumpBuffer");

		if (input.axisInput.y < 0)
			timers.StartTimer("platformFallThrough");

		if (timers.Active("jumpBuffer") && !timers.Expired("jumpBuffer") && 
			timers.Active("coyoteBuffer") && !timers.Expired("coyoteBuffer")) {

			velocity.y = jumpVelocityMax;	
			timers.SetActive("jumpBuffer", false);
			timers.SetActive("coyoteBuffer", false);

			// if jump button released before touching the ground
			if (!input.jump)
				velocity.y = Mathf.Min(velocity.y, jumpVelocityMin);

		} else if (input.jumpRelease)
			velocity.y = Mathf.Min(velocity.y,jumpVelocityMin);
		
		Vector2 deltaPosition = velocity * Time.deltaTime;

		// Dash
		if (input.dash && (!timers.Active("dashBuffer") || timers.Expired("dashBuffer")) && velocity.x != 0) {
			timers.StartTimer("dashBuffer");
			if (velocity.x < 0)
				deltaPosition += new Vector2(-dashDistance, 0);
			else if (velocity.x > 0)
				deltaPosition += new Vector2(dashDistance, 0);
			anim?.SetState(AnimState.Dash);
		}

		// wall jump
		if ((raycastCollider.collisionInfo.AnyLeft || raycastCollider.collisionInfo.AnyRight) && 
			!raycastCollider.collisionInfo.AnyBot && !raycastCollider.platformCollisionInfo.AnyBot &&
			input.jumpDown) {
			velocity.y = jumpVelocityMax;
		}

		// wall slide: touching a wall while in the air, also didn't change direction or jump
		if ((raycastCollider.collisionInfo.AnyLeft || raycastCollider.collisionInfo.AnyRight) && 
			!raycastCollider.collisionInfo.AnyBot && !raycastCollider.platformCollisionInfo.AnyBot &&
			oldVelocityX * velocity.x >= 0 && !input.jumpDown) {
			velocity.x = oldVelocityX;
			velocity.y = -wallSlideSpeed;
			Move(velocity * Time.deltaTime);
			return;
		}

		UpdateAnimStates();

		// terminal velocity
		velocity.y = Mathf.Clamp(velocity.y, -terminalVelocity, Mathf.Infinity);

		Move(deltaPosition);
		#endregion

		// // Shop
		// if (input.health && stats.getHealthUpgradeCost() <= currMoney) {
		// 	currMoney -= stats.getHealthUpgradeCost();
		// 	stats.incrementHealth();
		// } else if (input.health) {
		// 	// Not enough money
		// }

		// if (input.defense && stats.getDefenseUpgradeCost() <= currMoney) {
		// 	currMoney -= stats.getDefenseUpgradeCost();
		// 	stats.incrementDefense();
		// } else if (input.defense) {
		// 	// Not enough money
		// }

		// if (input.damage && stats.getDamageUpgradeCost() <= currMoney) {
		// 	currMoney -= stats.getDamageUpgradeCost();
		// 	stats.incrementDamage();
		// } else if (input.damage) {
		// 	// Not enough money
		// }
	}

	// only call this once per frame
	void Move(Vector2 deltaPosition) {
		raycastCollider.Move(deltaPosition, timers.Active("platformFallThrough") && !timers.Expired("platformFallThrough"));
	}

	void UpdateAnimStates() {
		if (input.axisInput.x != 0) {
			faceDir = (int) Mathf.Sign(input.axisInput.x);

			if (sprite != null)
				sprite.flipX = faceDir == -1;
		}

		if (anim != null) {
			AnimState state = anim.State;

			if (state != AnimState.Dash && state != AnimState.Hit) {
				if (!raycastCollider.collisionInfo.AnyBot && !raycastCollider.platformCollisionInfo.AnyBot && velocity.y < 0)
				{
					anim.SetState(AnimState.Fall);
				}
				else if (!raycastCollider.collisionInfo.AnyBot && !raycastCollider.platformCollisionInfo.AnyBot)
				{
					anim.SetState(AnimState.Jump);
				}
				else if (velocity.x != 0)
				{
					anim.SetState(AnimState.Run);
				}
				else
				{
					anim.SetState(AnimState.Idle);
				}
			}
		}
	}
}
