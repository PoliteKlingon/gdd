using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentProps : MonoBehaviour
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;

    public float GetX() { return x; }
    public float GetY() { return y; }
    public float GetZ() { return z; }

    public static EnvironmentProps Instance {
        get; private set;

    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
