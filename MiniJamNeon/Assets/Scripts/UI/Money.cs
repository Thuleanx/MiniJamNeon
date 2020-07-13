using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Money : MonoBehaviour
{
	[SerializeField] Text text;

	float targetAmount;
	float currentAmount;
	[SerializeField] float smoothTimeSeconds = 1f;
	float moneySmoothDampTemp = 0;

	void Start() {
		targetAmount = currentAmount = 0;
	}

	public void SetAmount(int amount) {
		targetAmount = amount;
	}

	void Update() {
		currentAmount = Mathf.SmoothDamp(currentAmount, targetAmount, ref moneySmoothDampTemp, smoothTimeSeconds);
		text.text = String.Format("{0}", (int) Math.Round(currentAmount));
	}
}