using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timers : MonoBehaviour
{
	Dictionary<string, float> 	timeToExpire = new Dictionary<string, float>();
	Dictionary<string, float> 	duration = new Dictionary<string, float>();
	Dictionary<string, bool> 	timerActive = new Dictionary<string, bool>();

	public bool RegisterTimer(string name, float duration) {
		if (!timeToExpire.ContainsKey(name)) {
			timeToExpire[name] = 0;
			this.duration[name] = duration;
			timerActive[name] = false;
		}
		return !timeToExpire.ContainsKey(name);
	}

	public void StartTimer(string name) {
		timeToExpire[name] = Time.time + duration[name];
		SetActive(name, true);
	}

	public bool Active(string name) { return timerActive[name]; }

	public void SetActive(string name, bool active) {
		timerActive[name] = active;
	}

	public bool Expired(string name) {
		return Time.time >= timeToExpire[name];
	}
}
