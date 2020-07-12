using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    private static ScreenShakeController instance;

    private float shakeTimeRemaining, shakePower, shakeFadeTime;
    private bool shaking;
    Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
       instance = this;
       shaking = false;        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {
            StartShake(0.5f, 0.2f);
        }
    }

    private void LateUpdate()
    {
        if(shakeTimeRemaining > 0) {
            transform.localPosition = originalPosition;
            shakeTimeRemaining -= Time.deltaTime;
            float xShift = Random.Range(-1.0f, 1.0f) * shakePower;
            float yShift = Random.Range(-1.0f, 1.0f) * shakePower;

            transform.localPosition += new Vector3(xShift, yShift, 0);
            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        } else if(shaking) {
            shaking = false;
            transform.localPosition = originalPosition;
        }
    }

    public void StartShake(float time, float power) {
        shakeTimeRemaining = time;
        shakePower = power;
        shakeFadeTime = power / time;

        if(!shaking) {
           shaking = true;
           originalPosition = transform.localPosition;
        }
    }
}
