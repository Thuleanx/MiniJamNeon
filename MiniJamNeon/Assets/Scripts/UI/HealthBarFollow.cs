using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFollow : MonoBehaviour
{
	[SerializeField] Slider slider;	
	HealthBar healthBar;
	[SerializeField] float smoothTimeSeconds = 1f;
	float currentSmoothDampTemp = 0;

	void Start() {
		healthBar = HealthBar.Instance;
	}

	void Update() {
		slider.value = Mathf.SmoothDamp(slider.value, healthBar.health, ref currentSmoothDampTemp, smoothTimeSeconds);
	}
}
