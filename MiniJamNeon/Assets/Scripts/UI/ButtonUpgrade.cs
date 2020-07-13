
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUpgrade: MonoBehaviour
{
	public void Upgrade(string name) {
		CurrencyController.Instance.TryUpdgrade(name);
	}
}