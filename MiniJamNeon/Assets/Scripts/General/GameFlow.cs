using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    protected static GameFlow instance;
    public static GameFlow Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<GameFlow>();
            return instance;
        }
    }

    public float playerSpeed = 10f;
    public float bulletSpeed = 12f;
    public float bulletRange = 2;
    public int bulletCount = 1;
    public GameObject player;
    public Camera camera;

    void Awake()
    {
        
    }

    void Start()
	{
        // game start logic
	}
}