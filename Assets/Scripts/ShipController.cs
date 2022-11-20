using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    
    [SerializeField] private GameObject[] leftThrusters;
    [SerializeField] private GameObject[] rightThrusters;
    [SerializeField] private GameObject[] frontThrusters;
    [SerializeField] private GameObject[] backThrusters;
    
    [SerializeField] private float speed = 10;

    private Rigidbody _rigidbody;

    private void SetThrusters(GameObject[] thrusters, bool value)
    {
        foreach (GameObject thruster in thrusters)
        {
            thruster.SetActive(value);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null) Debug.Log("Missing rigidbody component!");
        
        SetThrusters(frontThrusters, false);
        SetThrusters(backThrusters, false);
        SetThrusters(leftThrusters, false);
        SetThrusters(rightThrusters, false);
    }

    // Update is called once per frame
    void Update()
    {
        SetThrusters(frontThrusters, false);
        SetThrusters(backThrusters, false);
        SetThrusters(leftThrusters, false);
        SetThrusters(rightThrusters, false);
        
        if (Input.GetKey(forwardKey))
        {
            _rigidbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(backThrusters, true);
        }
        
        if (Input.GetKey(backwardKey))
        {
            _rigidbody.AddForce(transform.forward * -speed * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(frontThrusters, true);
        }
        
        if (Input.GetKey(rightKey))
        {
            _rigidbody.AddForce(transform.right * speed * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(leftThrusters, true);
        }
        
        if (Input.GetKey(leftKey))
        {
            _rigidbody.AddForce(transform.right * -speed * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(rightThrusters, true);
        }
    }
}
