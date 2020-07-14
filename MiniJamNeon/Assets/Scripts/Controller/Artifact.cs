using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        
    }

    public void Activate()
    {
        Debug.Log("here");
        myRenderer.enabled = true;
    }
}
