using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasDuration : MonoBehaviour
{
	[SerializeField] float durationSeconds; 
	[SerializeField] bool destroyOnExpire = false;
	float timeExpire;



	void Awake() {

	}

	void OnEnable() {
		timeExpire = Time.time + durationSeconds;
	}

	void Update()
	{
		if (timeExpire <= Time.time) {
			if (destroyOnExpire) 	Destroy(gameObject);
			else 					gameObject.SetActive(false);	
		}
	}
}