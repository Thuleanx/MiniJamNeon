using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    private PlayerController2D player;
    private Renderer myRenderer;

    private bool activated = false;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2D>();
    }

    public void Activate()
    {
        if (!activated)
        {
            activated = true;
            myRenderer.enabled = true;
            player.activatedArtifact();
        }
        
    }
}
