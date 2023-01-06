using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //brzdeni
        _rigidbody.AddForce(-_rigidbody.velocity * 2 * Time.deltaTime, ForceMode.Impulse);
        _rigidbody.AddTorque(-_rigidbody.angularVelocity.normalized * 100 * Time.deltaTime);
    }
}
