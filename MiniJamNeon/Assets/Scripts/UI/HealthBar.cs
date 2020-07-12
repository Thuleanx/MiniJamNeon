using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] Slider slider;
	[HideInInspector] public float health;

	void Start() {
		SetHealth(1f);
	}

	// from 0 to 1
	void SetHealth(float health) {
		this.health = health;
		slider.value = health;
	}
}
