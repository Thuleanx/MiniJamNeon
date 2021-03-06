﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	[SerializeField] GameObject teleporter;

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
	[SerializeField] float wallJumpCooldownSeconds;
	float platformFallThroughSeconds;
	#endregion

	int currMoney;

	// face direction to animate the sprite
	int faceDir;
	float lastTimeTouchWall;

	public int totalNumberArtifacts = 4;
	private int artifactsActivated = 0;

	void Awake() {
		lastTimeTouchWall = -1;

		CalculatePhysicsConstants();
    	currMoney = 0;
	}

	void Start() {
		raycastCollider = GetComponent<RaycastCollider2D>();
		input = GetComponent<InputManager>();
		timers = GetComponent<Timers>();
    	stats = GetComponent<PlayerStats>();
		sprite = GetComponent<SpriteRenderer>();
		anim = GetComponent<CharacterAnimationController>();

		// Init all timers
		timers.RegisterTimer("jumpBuffer", inputBufferTimeSeconds);
		timers.RegisterTimer("coyoteBuffer", coyoteTimeSeconds);
		timers.RegisterTimer("platformFallThrough", platformFallThroughSeconds);
		timers.RegisterTimer("dashBuffer", dashCooldownSeconds);

		// make teleporter sprite deactivated
		teleporter.GetComponent<Renderer>().enabled = false;
	}

	void CalculatePhysicsConstants() {
		gravity = 2 * maxJumpHeight / (timeToJumpApexSeconds * timeToJumpApexSeconds);	

		jumpVelocityMax = gravity * timeToJumpApexSeconds;
		jumpVelocityMin = Mathf.Sqrt(minJumpHeight * gravity);

		platformFallThroughSeconds =  Mathf.Sqrt(1f / gravity);
	}

	void Update() {
		// check for win condition
		if (Win()) {
			Debug.Log("win!");
			SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
		}

		if (TouchingWall())
			lastTimeTouchWall = Time.time;

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
		if (Time.time - lastTimeTouchWall <= wallJumpCooldownSeconds && input.jumpDown) {
			velocity.y = jumpVelocityMax;
		}

		// wall slide: touching a wall while in the air, also didn't change direction or jump
		if (TouchingWall() && oldVelocityX * velocity.x >= 0 && !input.jumpDown) {
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

			if (state != AnimState.Dash && state != AnimState.Hit && anim.State != AnimState.Shine) {
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

	// specifically, touching the wall while in the air
	bool TouchingWall() {
		return (raycastCollider.collisionInfo.AnyLeft || raycastCollider.collisionInfo.AnyRight) && 
			!raycastCollider.collisionInfo.AnyBot && !raycastCollider.platformCollisionInfo.AnyBot;
	}

	bool CloseToObject(GameObject other) {
		float x1 = this.transform.position.x;
		float y1 = this.transform.position.y;
		float x2 = other.transform.position.x;
		float y2 = other.transform.position.y;
		return Math.Abs(x1 - x2) <= 1 && Math.Abs(y1 - y2) <= 1;
	}

	public void activatedArtifact()
    {
		artifactsActivated++;
		Debug.Log("Activated " + artifactsActivated + " artifacts");
		if (artifactsActivated == totalNumberArtifacts)
        {
			// light up teleporter
			teleporter.GetComponent<Renderer>().enabled = true;
		}
    }

	bool Win() {
		return artifactsActivated == totalNumberArtifacts && CloseToObject(teleporter);
	}
}
