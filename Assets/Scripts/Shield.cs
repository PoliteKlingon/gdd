using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float _energy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //TODO kolik ubrat energie, pripadne co udelat dal
        Debug.Log("Hit shield");
    }

    public void SetEnergy(float amount)
    {
        _energy = amount;
    }
}
