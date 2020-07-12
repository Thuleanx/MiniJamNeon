using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFollow : MonoBehaviour
{
	[SerializeField] Slider slider;	
	[SerializeField] HealthBar healthBar;
	[SerializeField] float smoothTimeSeconds = 1f;
	float currentSmoothDampTemp = 0;

	void Start() {
	}

	void Update() {
		slider.value = Mathf.SmoothDamp(slider.value, healthBar.health, ref currentSmoothDampTemp, smoothTimeSeconds);
	}
}
