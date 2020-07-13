using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SetDamageOnStart : MonoBehaviour
{
	EnemyStats stats;
	Hitbox hitbox;

	void Start() {
		stats = GetComponent<EnemyStats>();
		hitbox = GetComponentInChildren<Hitbox>();
	}

	void Update() {
		if (hitbox != null && stats != null)
			hitbox.setDamage(stats.getDamage());
	}
}