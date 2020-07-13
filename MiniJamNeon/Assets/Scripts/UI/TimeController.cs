using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
	[SerializeField] Text textbox;
	public float timeElapsedSeconds;
	bool pause;

  public static TimeController clock = null;

	public void Pause() {
		pause = true;
	}

	void Start() {
		timeElapsedSeconds = 0;

    if(clock == null) {
        clock = this;
        DontDestroyOnLoad(this);
    }
	}

	void Update() {
		if (!pause) {
			timeElapsedSeconds += Time.deltaTime;
			textbox.text = String.Format("{0:D9}", (int) timeElapsedSeconds);
		}
	}
}
