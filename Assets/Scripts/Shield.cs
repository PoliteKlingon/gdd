using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameUtils.ShieldType type;
    [SerializeField] private float damageDivider = 10;
    [SerializeField] private float rocketDamage = 20;
    [SerializeField] private Health shipHealth;
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
        Debug.Log("Hit shield" + type);
    }

    public void SetEnergy(float amount)
    {
        _energy = amount;
    }

    public void DealDamage(float amount)
    {
        float healthDamage = 0;
        _energy -= amount;
        if (_energy <= 0)
        {   
            healthDamage = System.Math.Abs(_energy) / damageDivider;
            _energy = 0;
            shipHealth.DealDamage(healthDamage);
        }
    }
}
