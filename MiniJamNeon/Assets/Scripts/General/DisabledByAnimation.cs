

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DisabledByAnimation : MonoBehaviour
{
	public void Disable() {
		gameObject.SetActive(false);
	}
}