using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public static HealthBar Instance;
	[SerializeField] Slider slider;
	[HideInInspector] public float health;

	void Awake() {
		Instance = this;
	}

	void Start() {
		SetHealth(1f);
	}

	// from 0 to 1
	public void SetHealth(float health) {
		this.health = health;
		slider.value = health;
	}
}
