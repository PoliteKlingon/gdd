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
    
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float maxSpeed = 10;

    private Rigidbody _rigidbody;

    [SerializeField] private string rotationAxisX = "Mouse X";
    [SerializeField] private string rotationAxisY = "Mouse Y";

    private float _rotationX;
    private float _rotationY;

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
            _rigidbody.AddForce(transform.forward * acceleration * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(backThrusters, true);
        }
        
        if (Input.GetKey(backwardKey))
        {
            _rigidbody.AddForce(transform.forward * -acceleration * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(frontThrusters, true);
        }
        
        if (Input.GetKey(rightKey))
        {
            _rigidbody.AddForce(transform.right * acceleration * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(leftThrusters, true);
        }
        
        if (Input.GetKey(leftKey))
        {
            _rigidbody.AddForce(transform.right * -acceleration * Time.deltaTime, ForceMode.Impulse);
            SetThrusters(rightThrusters, true);
        }
        
        //brzdeni
        _rigidbody.AddForce(-_rigidbody.velocity * 2 * Time.deltaTime, ForceMode.Impulse);
        _rigidbody.AddTorque(-_rigidbody.angularVelocity.normalized * 100 * Time.deltaTime);
        
        //maxspeed clip bude zde
        
        //rotace mysi
        _rotationX = Input.GetAxis(rotationAxisX);
        _rotationY = Input.GetAxis(rotationAxisY);
        transform.Rotate(new Vector3(-_rotationY, _rotationX, 0));
        
        //skryt kurzor mysi
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
